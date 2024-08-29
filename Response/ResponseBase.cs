using System.Net;

namespace Estacionei.Response
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; } 
        public string? Message { get; set; }

        public static ResponseBase<T> SuccessResult(T data, string message,HttpStatusCode statusCode = HttpStatusCode.OK) => new ResponseBase<T> { Success = true, Data = data, Message = message, StatusCode = statusCode};
        //public static ResponseBase<T> NotFoundResult(string message) => new ResponseBase<T> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = message };

        public static ResponseBase<T> FailureResult(string message,HttpStatusCode statusCode) => new ResponseBase<T> { Success = false, Message = message, StatusCode = statusCode };
    }

}

