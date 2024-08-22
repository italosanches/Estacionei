namespace Estacionei.Response
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public static ResponseBase<T> SuccessResult(T data,string message) => new ResponseBase<T> { Success = true, Data = data , Message = message};
        public static ResponseBase<T> FailureResult(string message) => new ResponseBase<T> { Success = false, Message = message };
    }

}

