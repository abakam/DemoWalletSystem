using BetWalletApi.Models.Common;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Wallets;
using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Models.Users
{
    public class User : Entity
    {
        [Required]
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public Wallet? Wallet { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
