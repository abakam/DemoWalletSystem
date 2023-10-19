using BetWalletApi.Models.Wallets;

namespace BetWalletApi.Repositories.EFCore
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
