using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Iris.FrameCore
{
    public class HttpWebClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public HttpWebClient(
            IHttpClientFactory httpClientFactory,
            ILogger<HttpWebClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// POST 发送XML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strxml"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public async Task<(string returnstr, string errmsg)> PostXmlAsync(string url, string strxml, Encoding encoding, int timeOut = 10000)
        {
            var client = _httpClientFactory.CreateClient();
            string ret = null;
            string errmsg = null;
            encoding = encoding ?? Encoding.GetEncoding("GBK");
            try
            {
                var data = encoding.GetBytes(strxml);
                HttpContent hc = new ByteArrayContent(data);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                hc.Headers.Add("Timeout", timeOut.ToString());
                hc.Headers.Add("KeepAlive", "true");

                var r = await client.PostAsync(url, hc);
                if (r.StatusCode != HttpStatusCode.OK)
                {
                    errmsg = await r.Content.ReadAsStringAsync();
                    return (ret, errmsg);
                }
                ret = await r.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
            return (ret, errmsg);
        }

        /// <summary>
        /// POST 发送JSON
        /// </summary>
        /// <typeparam name="T">post数据类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="entity">POST数据</param>
        /// <returns></returns>
        public async Task<(HttpStatusCode code, string ret)> PostJsonAsync(string url, string body, int timeOut = 10000, string mediaType = "application/json")
        {
            var client = _httpClientFactory.CreateClient();
            HttpStatusCode code = HttpStatusCode.OK;
            string ret;
            try
            {
                HttpContent hc = new StringContent(body);
                hc.Headers.Add("Timeout", timeOut.ToString());
                hc.Headers.Add("KeepAlive", "true");
                hc.Headers.ContentType.MediaType = mediaType;
                var r = await client.PostAsync(url, hc);
                if (!r.IsSuccessStatusCode)
                {
                    code = r.StatusCode;
                    ret = await r.Content.ReadAsStringAsync();
                    return (code, ret);
                }
                ret = await r.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                return (code, e.Message);
            }
            return (code, ret);
        }

        /// <summary>
        /// GET 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <returns>Item1 返回数据 Item2 错误信息</returns>
        public async Task<(string returnstr, string errmsg)> GetAsync(string url, string query, int timeOut = 10000)
        {
            var client = _httpClientFactory.CreateClient();
            string ret = "";
            string errmsg = null;
            client.Timeout = new TimeSpan(0, 0, 0, 0, timeOut);

            try
            {
                var r = await client.GetAsync(url + query);
                if (!r.IsSuccessStatusCode)
                {
                    errmsg = await r.Content.ReadAsStringAsync();
                    return (ret, errmsg);
                }
                ret = await r.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
            return (ret, errmsg);
        }

    }
}
