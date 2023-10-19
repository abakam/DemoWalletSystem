using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Enums;

namespace BetWalletApi.Models.Ledgers
{
    public class Ledger : Entity
    {
        public Guid UserId { get; set; }  // Foreign Key User 
        public Decimal? Credit { get; set; }
        public Decimal? Debit { get; set;}
        public Decimal CurrentBalance { get; set; }
        public WalletFlow TransactionType { get; set; }

    }
}
