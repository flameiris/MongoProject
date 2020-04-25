
//namespace Iris.Ocelot.MiddleWares.Authender.Middleware
//{
//    using Microsoft.AspNetCore.Http;
//    using Newtonsoft.Json;
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using Microsoft.Extensions.Options;
//    using global::Ocelot.Middleware;

//    public class CustomAuthenticationMiddleware : OcelotMiddleware
//    {
//        private readonly OcelotRequestDelegate _next;
//        private readonly ILoginLogRepository _repository;
//        private readonly IOcelotLogger _logger;
//        private readonly List<string> _anoyousList = new List<string>();
//        public CustomAuthenticationMiddleware(OcelotRequestDelegate next,
//                IOcelotLoggerFactory loggerFactory,
//                ILoginLogRepository repository,
//                IOptions<List<string>> anoyousList
//            ) : base(loggerFactory.CreateLogger<CustomAuthenticationMiddleware>())
//        {
//            _next = next;
//            _repository = repository;
//            _anoyousList = anoyousList.Value;
//            _logger = loggerFactory.CreateLogger<CustomAuthenticationMiddleware>();
//        }

//        public async Task Invoke(DownstreamContext context)
//        {
//            if (!context.DownstreamReRoute.IsAuthenticated)
//            {
//                await _next.Invoke(context);
//                return;
//            };
//            var result = await GetAuthenticationResult(context.HttpContext);
//            if (result.Code == 200)
//            {
//                await _next.Invoke(context);
//                return;
//            }
//            await SetResponseOnHttpContext(context.HttpContext, result);
//            Logger.LogInformation($"请求验证不通过，{context.DownstreamRequest.AbsolutePath}");
//        }

//        private async Task<ResponseBodyModel> GetAuthenticationResult(HttpContext context)
//        {
//            var requestPath = context.Request.Path.Value.ToLower();
//            var reqeustQuery = context.Request.QueryString.Value;
//            // 在一开始就需要记录参数
//            StringBuilder logBuilder = new StringBuilder();
//            logBuilder.Append($"{Environment.NewLine}URL:{Environment.NewLine}");
//            logBuilder.Append(context.Request.Host + requestPath + reqeustQuery + Environment.NewLine);
//            var sign = context.Request.Headers["Sign"].ToString();
//            var strTimestamp = context.Request.Headers["Timestamp"].ToString();
//            var platform = context.Request.Headers["Platform"].ToString();
//            var token = context.Request.Headers["Token"].ToString();
//            var Version = context.Request.Headers["VersionName"].ToString();
//            Version = string.IsNullOrWhiteSpace(Version) ? "0" : Version;
//            //int phoneversion = Convert.ToInt32(Version.Replace(".", ""));
//            logBuilder.Append($"Headers:{Environment.NewLine}");
//            logBuilder.Append($"Sign={sign}{Environment.NewLine}");
//            logBuilder.Append($"Timestamp={strTimestamp}{Environment.NewLine}");
//            logBuilder.Append($"Platform={platform}{Environment.NewLine}");
//            logBuilder.Append($"phoneversion={Version}{Environment.NewLine}");
//            logBuilder.Append($"Token={token}{Environment.NewLine}");

//            var method = context.Request.Method.ToUpper();
//            logBuilder.Append($"Method={method}{Environment.NewLine}");

//            var body = string.Empty;
//            if (string.IsNullOrWhiteSpace(sign) || string.IsNullOrWhiteSpace(strTimestamp) || string.IsNullOrWhiteSpace(platform))
//            {
//                return new ResponseBodyModel(401, "缺少请求头！");
//            }

//            if (!new[] { "iOS", "Android", "Web" }.Contains(platform))
//            {
//                return new ResponseBodyModel(401, "Platform参数不正确！");
//            }

//            if (platform != "Web")
//            {
//                var havePath = false;
//                foreach (var item in _anoyousList)
//                {
//                    if (requestPath.Contains(item))
//                    {
//                        havePath = true;
//                        break;
//                    }
//                }
//                if (!havePath && string.IsNullOrWhiteSpace(token))
//                {
//                    return new ResponseBodyModel(401, "缺少Token");
//                }
//            }

//            SortedDictionary<string, string> signDic = signDic = new SortedDictionary<string, string>(StringComparer.Ordinal);
//            signDic.Add("Timestamp", strTimestamp);
//            signDic.Add("Platform", platform);

//            if (!string.IsNullOrWhiteSpace(token))
//            {
//                signDic.Add("Token", token);
//            }

//            signDic.Add("PublicKey", BHValidatorExtension.GetPublicKey(platform));

//            var timestamp = Convert.ToDouble(strTimestamp);
//            var timespan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds - timestamp;
//            if (timespan > 300 || timespan < -300)
//            {
//                return new ResponseBodyModel(401, "请求超时！");
//            }

//            //获取Post请求的Body
//            var resposeBody = await GetBody(method, context);
//            if (!resposeBody.Item2)
//                return new ResponseBodyModel(501, "请求长度超过限制！");
//            body = resposeBody.Item1;
//            logBuilder.Append($"Body:{Environment.NewLine}");
//            logBuilder.Append(body + Environment.NewLine);
//            _logger.LogInformation(logBuilder.ToString());

//            var signStatus = VerifySign(body, context.Request.Query, signDic, sign);
//            if (signStatus == -1)
//            {
//                return new ResponseBodyModel(401, "校验失败！");
//            }
//            else if (signStatus == 1)
//            {
//                return new ResponseBodyModel(200);
//            }

//            if (platform.Equals("Web", StringComparison.CurrentCultureIgnoreCase))
//            {
//                return new ResponseBodyModel(200);
//            }

//            //请求频繁
//            if (IsHaveRequestfrequently(method, requestPath, body, token, timestamp, reqeustQuery))
//            {
//                return new ResponseBodyModel(296, "您操作太快了，小v跟不上啦~>_<");
//            }

//            if (!string.IsNullOrEmpty(token))
//            {
//                var loginStatus = await _repository.GetLoginStatus(token);
//                switch (loginStatus)
//                {
//                    case LoginStatusType.Logined:
//                        return new ResponseBodyModel(200);
//                    case LoginStatusType.LogOut:
//                        return new ResponseBodyModel(295, "账号已经退出登录");
//                    case LoginStatusType.ToBeKicked:
//                        return new ResponseBodyModel(294, "账号已在其他设备登录");
//                    case LoginStatusType.Expire:
//                        return new ResponseBodyModel(293, "登录过期");
//                }
//                return new ResponseBodyModel(295, "账号已经退出登录");
//            }
//            return new ResponseBodyModel(200);
//        }

//        public async Task SetResponseOnHttpContext(HttpContext context, ResponseBodyModel responseModel)
//        {
//            using (Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(responseModel))))
//            {
//                await stream.CopyToAsync(context.Response.Body);
//            }
//        }

//        private bool IsHaveRequestfrequently(string method, string requestPath, string body, string token, double timestamp, string requestQuery)
//        {
//            //path是没有参数的
//            string reqPath = string.Empty;
//            //路径转换器
//            Func<string, string, string, string> pathFormat = (path, httpMethod, query) => $"{path}—{httpMethod}—{query}";

//            if (method == "GET")
//            {
//                reqPath = pathFormat(requestPath, method, requestQuery.ToString());
//            }

//            if (method == "POST" || method == "PUT")
//            {
//                reqPath = pathFormat(requestPath, method, body.ToMd5());
//            }

//            _logger.LogInformation("请求路径:" + reqPath);

//            var TimeOutKey = string.Format(RedisKeys.TimeOutRedisKey, token, timestamp, reqPath);
//            if (RedisHelper.Exists(TimeOutKey))
//            {
//                return true;
//            }
//            else
//            {
//                RedisHelper.Set(TimeOutKey, 1, 300);
//            }
//            return false;
//        }

//        private async Task<(string, bool)> GetBody(string method, HttpContext context)
//        {
//            string Body = string.Empty;
//            try
//            {
//                if (method == "POST" || method == "PUT")
//                {
//                    var contentType = context.Request.ContentType.ToUpper();
//                    var contentLength = context.Request.ContentLength;
//                    if (contentLength.HasValue && contentLength > 0)
//                    {
//                        if (contentType.Contains("JSON"))
//                        {
//                            if (contentLength <= int.MaxValue)
//                            {
//                                using (var mem = new MemoryStream())
//                                {
//                                    context.Request.Body.Position = 0;
//                                    context.Request.Body.CopyTo(mem);
//                                    mem.Position = 0;
//                                    using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
//                                    {
//                                        Body = await reader.ReadToEndAsync();
//                                    }
//                                }
//                                return (Body, true);
//                            }
//                            else
//                            {
//                                return ("", false);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.LogError(e.Message, e);
//            }
//            return ("", true);
//        }

//        private int VerifySign(string body, IQueryCollection Query, SortedDictionary<string, string> signDic, string sign)
//        {
//            if (!string.IsNullOrEmpty(body))
//            {
//                var bodyDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
//                foreach (var item in bodyDic)
//                {
//                    var value = string.Empty;
//                    if (item.Value is string || item.Value is ValueType)
//                    {
//                        value = item.Value.ToString();
//                    }
//                    else
//                    {
//                        // 目前只要是list或对象，就让校验通过，不走其他校验
//                        return 1;
//                    }
//                    signDic.Add(item.Key, value);
//                }
//            }
//            foreach (var arg in Query)
//            {
//                signDic.Add(arg.Key, arg.Value);
//            }
//            //校验不通过
//            if (!BHValidatorExtension.VerifySign(signDic, sign))
//            {
//                return -1;
//            }
//            //校验通过
//            return 0;
//        }
//    }
//}
