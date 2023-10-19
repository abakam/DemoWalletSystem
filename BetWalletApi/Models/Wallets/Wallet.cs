using BetWalletApi.Models.Common;
using BetWalletApi.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Models.Wallets
{
    public class Wallet : Entity
    {
        [Required]
        public Decimal Balance { get; set; }
        [Required]
        public Guid UserId { get; set; } 
        public User? User { get; set; }
    }
}
