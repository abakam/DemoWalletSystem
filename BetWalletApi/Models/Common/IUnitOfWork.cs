using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using BetWalletApi.Repositories.EFCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BetWalletApi.Models.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IWalletRepository Wallets { get; }
        ITransactionRepository Transactions { get; }
        ILedgerRepository Ledgers { get; }
        BetWalletDbContext DbContext { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        void Save();

    }
}
