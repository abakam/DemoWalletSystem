using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Enums;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Models.Transactions
{
    public class Transaction : Entity
    {
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required] 
        public TransactionStatus TransactionStatus { get; set;  }
        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public Guid UserId { get; set; }   
        public User? User { get; set; }
        public Ledger? Ledger { get; set; }
    }
}
