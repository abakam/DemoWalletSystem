namespace BetWalletApi.DTOs.Requests
{
    public class ApproveWithdrawalRequest : InitiateWithdrawalRequest
    {
        public string TransactionId { get;set; }
    }
}
