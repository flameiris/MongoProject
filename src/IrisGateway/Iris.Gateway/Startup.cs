using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Iris.MongoDB;
using Iris.MongoDB.Extensions;
using Iris.Ocelot.Extensions;
using Iris.Ocelot.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Iris.Gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        public Startup(IHostingEnvironment env)
        {
            Env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(Env.IsProduction() ? "appsettings.json" : $"appsettings.{Env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }



        public void ConfigureServices(IServiceCollection services)
        {
            AddMongoDBServiceCollectionExtensions.AddMongoDB(services, Configuration, Env);

            var authenticationProviderKey = "GatewayKey";

            services
                .AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                {
                    //是否启用https
                    options.RequireHttpsMetadata = false;
                    //配置授权认证的地址
                    options.Authority = $"http://localhost:10000";
                    //资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
                    options.ApiName = "Gateway";
                    options.ApiSecret = "Gateway.Secret";
                    options.SupportedTokens = SupportedTokens.Both;
                });




            services.AddOcelot()
                //注入自定义MongoDb读取Ocelot配置
                .AddMongoOcelot();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseOcelot().Wait();
        }
    }









    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string userName = "", requestInfo = "", responseInfo = "";
            var originalBodyStream = context.Response.Body;
            var stopwach = Stopwatch.StartNew();
            try
            {

                requestInfo = await FormatRequest(context.Request);

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);
                    stopwach.Stop();

                    responseInfo = await FormatResponse(context.Response);
                    await responseBody.CopyToAsync(originalBodyStream);
                }

                userName = Convert.ToString(context.Items["userName"]);
                var logMsg = $@"{userName} 请求信息: {requestInfo}{Environment.NewLine}响应信息: {responseInfo}{Environment.NewLine}耗时: {stopwach.ElapsedMilliseconds}ms";
                Console.WriteLine(logMsg);


            }
            catch (Exception ex)
            {
                stopwach.Stop();
                if (ex != null)
                {
                    var logMsg = $@"{userName} 请求信息: {requestInfo}{Environment.NewLine}异常: {ex.ToString()}{Environment.NewLine}耗时: {stopwach.ElapsedMilliseconds}ms";

                    var bytes = Encoding.UTF8.GetBytes(logMsg);
                    await originalBodyStream.WriteAsync(bytes, 0, bytes.Length);

                }
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $" {request.Method} {request.Scheme}://{request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }

}