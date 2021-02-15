using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public static class TokenOperation
    {
        public static string SetToken(string id)
        {
            return SetToken(id, Configurations.TOKEN_TIME);
        }

        public static string SetToken(string id, int minutes)
        {
            string token = Utils.AesEncryptor_Hex(id+DateTime.Now.ToString(), Configurations.SecretKey);
            HttpRuntimeCache cache = new HttpRuntimeCache();
            cache.Add(token, id,minutes*60);
            return token;
        }

        public static string GetIdByToken(string token)
        {
            HttpRuntimeCache cache = new HttpRuntimeCache();
            string id = string.Empty;
            if(cache.ContainsKey<string>(token))
            {
                id = cache.Get<string>(token);
                cache.Add(token, id, Configurations.TOKEN_TIME * 60);
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
