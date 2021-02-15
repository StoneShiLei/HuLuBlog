using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Stone.HuLuBlog.Infrastructure
{
    public class ResponseModel
    {
        /// <summary>
        /// 状态标识
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        public string Token { get; set; }

        public ResponseModel()
        {
            IsSuccess = true;
            Message = string.Empty;
            object Data = new object();
            Token = string.Empty;
        }

        public ResponseModel(bool isSuccess,string message,object data,string token = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            Token = token;
        }

        /// <summary>
        /// 将返回结果实体Json化，前端需JSON.parse(data)
        /// </summary>
        /// <returns></returns>
        public string ConvertJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ResponseModel Success(string message = null,object data = null,string token = null)
        {
            return new ResponseModel(true, message, data, token);
        }

        public static ResponseModel Error(string message = null)
        {
            return new ResponseModel(false, message, null, string.Empty);
        }
    }
}
