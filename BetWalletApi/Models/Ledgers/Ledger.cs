using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Enums;
using BetWalletApi.Models.Transactions;
using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Models.Ledgers
{
    public class Ledger : Entity
    {
        [Required]
        public Decimal Credit { get; set; }
        [Required]
        public Decimal Debit { get; set;}
        [Required]
        public Decimal CurrentBalance { get; set; }
        [Required]
        public WalletFlowType TransactionType { get; set; }

        [Required]
        public Guid TransactionId { get; set; }  
        public Transaction? Transaction { get; set; }

    }
}
