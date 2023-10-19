using BetWalletApi.Models.Ledgers;

namespace BetWalletApi.Repositories.EFCore
{
    public class LedgerRepository : BaseRepository<Ledger>, ILedgerRepository
    {
        public LedgerRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
