namespace BetWalletApi.DTOs.Responses
{
    public class FundWalletResponse
    {
        public decimal Amount { get; set; }
        public string? Username { get; set; }
        public Guid TransactionId { get; set; }
        public string? TransactionType { get; set; }
    }
}
