namespace BetWalletApi.DTOs.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Result { get; set; }

        public static BaseResponse<T> WithSuccess(T result)
        {
            return new BaseResponse<T> { Success = true, Result = result };
        }

        public static BaseResponse<T> WithError(string message, int errorCode)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
