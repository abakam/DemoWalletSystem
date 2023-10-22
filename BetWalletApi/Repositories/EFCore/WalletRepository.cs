using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using Microsoft.EntityFrameworkCore;

namespace BetWalletApi.Repositories.EFCore
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        private readonly BetWalletDbContext _dbContext;
        public WalletRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wallet> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Set<Wallet>().Where(w => w.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
