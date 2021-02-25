using Com.Stone.HuLuBlog.Infrastructure.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public static class RecaptchaV3Operation
    {
        /// <summary>
        /// 验证recaptcha
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public static bool ReCaptchaPassed(string token,string userIP)
        {
            if (token.IsNullOrEmpty()) return false;
            var ip = userIP;
            string secret = Configurations.SECRET_KEY;
            var request = (HttpWebRequest)WebRequest.Create(Configurations.RECAPTCHA_URL);
            var postData = string.Format("secret={0}&response={1}&remoteip={2}", secret, token, ip);
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) return false;
            var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
            JObject jObject = JsonConvert.DeserializeObject(responseStr) as JObject;

            if (!(jObject is JObject)) return false;
            if (jObject["success"]?.ToString().ToLower() != "true") return false;

            double score = Convert.ToDouble(jObject["score"]);
            return score > 0.5;
        }
    }
}
