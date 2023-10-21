using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.DTOs.Requests
{
    public class InitiateWithdrawalRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public Decimal Amount { get; set; }
        [Required]
        public string TransactionType { get; set; }

    }
}
