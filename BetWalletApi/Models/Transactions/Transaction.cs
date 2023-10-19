using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Enums;

namespace BetWalletApi.Models.Transactions
{
    public class Transaction : Entity
    {
        public TransactionType TransactionType { get; set; }  // Convert Enum to String with EF Core
        public string TransactionReference { get; set; }    // External input - but should be unique
        public Decimal? Amount { get; set; }
        public Guid UserId { get; set; }   // Foreign Key User 
        public bool IsPostedToLedger { get; set; }
    }
}
