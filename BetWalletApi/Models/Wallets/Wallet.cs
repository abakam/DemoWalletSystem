using BetWalletApi.Models.Common;

namespace BetWalletApi.Models.Wallets
{
    public class Wallet : Entity
    {
        public Guid UserId { get; set; }  // Foreign Key User Entity
        public Decimal? Balance { get; set; }
    }
}
