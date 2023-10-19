using BetWalletApi.Models.Common;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.Repositories.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly BetWalletDbContext _dbContext;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction? _entityTransaction;

        public IUserRepository Users { get; }

        public IWalletRepository Wallets { get; }

        public ITransactionRepository Transactions { get; }

        public ILedgerRepository Ledgers { get; }

        public UnitOfWork(BetWalletDbContext dbContext, 
            IUserRepository users,
            IWalletRepository wallets, 
            ITransactionRepository transactions,
            ILedgerRepository ledgers)
        {
            _dbContext = dbContext;
            Users = users;
            Wallets = wallets;
            Transactions = transactions;
            Ledgers = ledgers;
        }

        public BetWalletDbContext DbContext
        { 
            get { return _dbContext; } 
        }

        public void BeginTransaction()
        {
            if (_entityTransaction == null)
            {
                _entityTransaction = _dbContext.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_entityTransaction == null) throw new ArgumentNullException(nameof(_entityTransaction));

            _entityTransaction.Commit();
        }


        public void Rollback()
        {
            _entityTransaction?.Rollback();
            _entityTransaction?.Dispose();
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                foreach(var validationError in dbEx.ValidationResult.MemberNames)
                {
                    _errorMessage += string.Format("Property: {0} Error: {1}", validationError, dbEx.Message) + Environment.NewLine;
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
