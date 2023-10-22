using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Enums;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;

namespace BetWalletApi.BackgroundServices
{
    public class FundWalletTransactionService : BackgroundService
    {
        private readonly ILogger<FundWalletTransactionService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FundWalletTransactionService(ILogger<FundWalletTransactionService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Starting Background: FundWalletTransactionService");

            stoppingToken.Register(() => _logger.LogDebug("FundWalletTransactionService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("FundWalletTransactionService backgroung task is running.");

                await PostInitiatedFundWalletTransactionsToLedger();
                await Task.Delay(1000, stoppingToken);     /// delayed time can be externalised in App settings 

            }
        }

        private async Task PostInitiatedFundWalletTransactionsToLedger()
        {
            _logger.LogDebug("Posting Initiated Fund Wallet Transactions to Ledger");

            var initiatedFundWalletTransactions = await GetInitiatedFundWalletTransactionsAsync();

            foreach (var transaction in initiatedFundWalletTransactions)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                    try
                    {
                        var existingWallet = await unitOfWork.Wallets.GetByUserIdAsync(transaction.UserId);

                        if (existingWallet != null)
                        {

                            // Begin Transaction
                            unitOfWork.BeginTransaction();
                            var currentBalance = existingWallet.Balance;

                            // Update wallet balance
                            existingWallet.Balance = currentBalance + transaction.Amount;
                            existingWallet.LastModified = DateTime.UtcNow;
                            unitOfWork.Wallets.Update(existingWallet);
                            unitOfWork.Save();

                            // Update transaction status
                            transaction.TransactionStatus = TransactionStatus.Approved;
                            transaction.LastModified = DateTime.UtcNow;
                            unitOfWork.Transactions.Update(transaction);
                            unitOfWork.Save();

                            // Post to ledger
                            var newLedger = new Ledger
                            {
                                TransactionId = transaction.Id,
                                CurrentBalance = currentBalance,
                                TransactionType = WalletFlowType.Credit,
                                Debit = 0,
                                Credit = transaction.Amount,
                                Created = DateTime.UtcNow,
                                LastModified = DateTime.UtcNow
                            };
                            unitOfWork.Ledgers.Add(newLedger);
                            unitOfWork.Save();
                            unitOfWork.Commit();
                        }
                    }
                    catch (Exception exception)
                    {
                        unitOfWork.Rollback();
                        _logger.LogCritical("Error while posting Initiated Fund Wallet Transaction with Transaction Id: " + transaction.Id, exception);
                    }
                }

            }
        }

        private async Task<IEnumerable<Transaction>> GetInitiatedFundWalletTransactionsAsync()
        {
            IEnumerable<Transaction> transactions = new List<Transaction>();

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var transactionRepository = scope.ServiceProvider.GetService<ITransactionRepository>();

                    transactions = await transactionRepository.ListAsync(t =>
                                               (t.TransactionType == TransactionType.Winning ||
                                               t.TransactionType == TransactionType.Bonus ||
                                               t.TransactionType == TransactionType.Deposit) && 
                                               t.TransactionStatus == TransactionStatus.Initiated);
                }


                return transactions;
            }
            catch (Exception exception)
            {
                _logger.LogCritical("Error while fetching initiated fund wallet transactions: ", exception);
            }

            return transactions;
        }
    }
}
