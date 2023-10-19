using BetWalletApi.Models.Transactions;

namespace BetWalletApi.Repositories.EFCore
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
