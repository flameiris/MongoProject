using Iris.Infrastructure.Helpers;
using Iris.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Common
{
    public class BaseResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public BaseResponse(BusinessStatusType code) : this(code, EnumberHelper.GetEnumDescription(code))
        {

        }

        public BaseResponse(BusinessStatusType code, string message) : this(code, message, null)
        {
        }

        public BaseResponse(BusinessStatusType code, string message, object data)
        {
            Code = (int)code;
            Message = message;
            Data = data;
        }



        public static BaseResponse GetBaseResponse(BusinessStatusType code)
        {
            return new BaseResponse(code);
        }

        public static BaseResponse GetBaseResponse(BusinessStatusType code, string message)
        {
            return new BaseResponse(code, message);
        }

        public static BaseResponse GetBaseResponse(BusinessStatusType code, string message, object data)
        {
            return new BaseResponse(code, message, data);
        }
    }
}
