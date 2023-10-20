namespace BetWalletApi.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
    }
}
