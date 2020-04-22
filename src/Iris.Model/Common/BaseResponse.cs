using Iris.Infrastructure.Helpers;
using Iris.Models.Enums;

namespace Iris.Models.Common
{
    //public class BaseResponse<T>
    //    where T : new()
    //{
    //    public int Code { get; set; }
    //    public string Message { get; set; }
    //    public T Response { get; set; }

    //    public BaseResponse(BusinessStatusType code) : this(code, EnumberHelper.GetEnumDescription(code))
    //    {

    //    }

    //    public BaseResponse(BusinessStatusType code, string message) : this(code, message, new T())
    //    {
    //    }

    //    public BaseResponse(BusinessStatusType code, string message, T response)
    //    {
    //        Code = (int)code;
    //        Message = message;
    //        Response = response;
    //    }



    //    public static BaseResponse<T> GetBaseResponse(BusinessStatusType code)
    //    {
    //        return new BaseResponse<T>(code);
    //    }

    //    public static BaseResponse<T> GetBaseResponse(BusinessStatusType code, string message)
    //    {
    //        return new BaseResponse<T>(code, message);
    //    }

    //    public static BaseResponse<T> GetBaseResponse(BusinessStatusType code, string message, T response)
    //    {
    //        return new BaseResponse<T>(code, message, response);
    //    }
    //}

    public class BaseResponse
    {
        public BaseResponse()
        {

        }
        private BusinessStatusType _code;
        public BusinessStatusType Code
        {
            get { return (BusinessStatusType)_code; }
            set { _code = value; }
        }

        public string Message { get; set; }
        public object Response { get; set; }

        public BaseResponse(BusinessStatusType code) : this(code, EnumberHelper.GetEnumDescription(code))
        {

        }

        public BaseResponse(BusinessStatusType code, string message) : this(code, message, null)
        {
        }

        public BaseResponse(BusinessStatusType code, string message, object response)
        {
            Code = code;
            Message = message;
            Response = response;
        }



        public static BaseResponse GetBaseResponse(BusinessStatusType code)
        {
            return new BaseResponse(code);
        }

        public static BaseResponse GetBaseResponse(BusinessStatusType code, string message)
        {
            return new BaseResponse(code, message);
        }

        public static BaseResponse GetBaseResponse(BusinessStatusType code, object response)
        {
            return new BaseResponse(code, "", response);
        }

        public static BaseResponse GetBaseResponse(BusinessStatusType code, string message, object response)
        {
            return new BaseResponse(code, message, response);
        }
    }
}
