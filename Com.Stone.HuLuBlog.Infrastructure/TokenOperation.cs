using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Web;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public static class TokenOperation
    {
        public static string SetToken(string id, int minutes)
        {
            string token = Utils.AesEncryptor_Hex(id+DateTime.Now.ToString(), Configurations.PASSWORD_SECRET_KEY);
            HttpRuntimeCache cache = new HttpRuntimeCache();
            cache.Add(token, id,minutes*60);
            return token;
        }

        /// <summary>
        /// 通过token获取用户id，同时刷新用户token缓存
        /// </summary>
        /// <param name="token"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string GetIdAndRefreshToken(string token,int minutes)
        {
            HttpRuntimeCache cache = new HttpRuntimeCache();
            string id = string.Empty;
            if(cache.ContainsKey<string>(token))
            {
                id = cache.Get<string>(token);
                cache.Add(token, id, minutes * 60);
            }

            return id;
        }

        public static void DeleteCacheByToken(string token)
        {
            HttpRuntimeCache cache = new HttpRuntimeCache();
            cache.Remove<string>(token);
        }

    }
}
