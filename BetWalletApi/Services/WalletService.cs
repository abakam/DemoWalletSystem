using Azure.Core;
using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;
using BetWalletApi.Helpers;
using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Constants;
using BetWalletApi.Models.Common.Enums;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;

namespace BetWalletApi.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IUnitOfWork unitOfWork, ILogger<WalletService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<CreateWalletResponse>> CreateWalletAsync(CreateWalletRequest request)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

                if (user != null)
                {
                    return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.USERNAME_ALREADY_EXISTS, ResponseStatusCodes.ALREADY_EXISTS);
                }

                var newUser = new User
                {
                    UserName = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                var newWallet = new Wallet
                {
                    UserId = newUser.Id,
                    Balance = 0.00m,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,

                };

                _unitOfWork.BeginTransaction();
                _unitOfWork.Users.Add(newUser);
                _unitOfWork.Save();
                _unitOfWork.Wallets.Add(newWallet);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                var createWalletResponse = new CreateWalletResponse
                {
                    WalletId = newWallet.Id,
                    Balance = newWallet.Balance,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    Username = newUser.UserName
                };

                return BaseResponse<CreateWalletResponse>.WithSuccess(createWalletResponse);
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
                _logger.LogError("Error while creating wallet: ", exception);
                return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);

            }

        }

        public async Task<BaseResponse<FundWalletResponse>> FundWalletAsync(string username, FundWalletRequest request)
        {
            if (!Utils.IsFundTransactionType(request.TransactionType))
            {
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INVALID_TRANSACTION_TYPE, ResponseStatusCodes.BAD_REQUEST);
            }

            if (request.Amount < 0)
            {
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INVALID_AMOUNT, ResponseStatusCodes.BAD_REQUEST);
            }

            try
            {
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(username);

                if (existingUser == null)
                {
                    return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var existingWallet = await _unitOfWork.Wallets.GetByUserIdAsync(existingUser.Id);

                if (existingWallet == null)
                {
                    return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var newTransaction = new Transaction
                {
                    UserId = existingUser.Id,
                    TransactionType = Utils.ConvertStringToTransactionType(request.TransactionType),
                    Amount = request.Amount,
                    TransactionStatus = TransactionStatus.Initiated,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(newTransaction);
                _unitOfWork.Save();

                var fundWalletResponse = new FundWalletResponse
                {
                    Amount = request.Amount,
                    TransactionId = newTransaction.Id,
                    TransactionType = request.TransactionType,
                    Username = username
                };

                return BaseResponse<FundWalletResponse>.WithSuccess(fundWalletResponse);

            }
            catch (Exception exception)
            {
                _logger.LogError("Error while funding wallet: ", exception);
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);
            }


        }

        public async Task<BaseResponse<CreateWalletResponse>> GetWalletDetailsAsync(string username)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(username);

                if (existingUser == null)
                {
                    return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var existingWallet = await _unitOfWork.Wallets.GetByUserIdAsync(existingUser.Id);

                if (existingWallet == null)
                {
                    return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var walletDetailsResponse = new CreateWalletResponse
                {
                    WalletId = existingWallet.Id,
                    Balance = existingWallet.Balance,
                    Email = existingUser.Email,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Username = existingUser.UserName
                };

                return BaseResponse<CreateWalletResponse>.WithSuccess(walletDetailsResponse);

            }
            catch (Exception exception)
            {
                _logger.LogError("Error while geting wallet details: ", exception);
                return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.INVALID_AMOUNT, ResponseStatusCodes.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<BaseResponse<ApproveWithdrawalRequest>> InitiateWithdrawalAsync(string username, InitiateWithdrawalRequest request)
        {

            if (!Utils.IsWithdrawTransactionType(request.TransactionType))
            {
                return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.INVALID_TRANSACTION_TYPE, ResponseStatusCodes.BAD_REQUEST);
            }

            if (request.Amount < 0)
            {
                return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.INVALID_AMOUNT, ResponseStatusCodes.BAD_REQUEST);
            }

            try
            {
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(username);

                if (existingUser == null)
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var newTransaction = new Transaction
                {
                    UserId = existingUser.Id,
                    TransactionType = Utils.ConvertStringToTransactionType(request.TransactionType),
                    Amount = request.Amount,
                    TransactionStatus = TransactionStatus.Initiated,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(newTransaction);
                _unitOfWork.Save();

                var initiateWithdrawalResponse = new ApproveWithdrawalRequest
                {
                    Amount = request.Amount,
                    TransactionId = newTransaction.Id.ToString(),
                    TransactionType = request.TransactionType,
                };

                return BaseResponse<ApproveWithdrawalRequest>.WithSuccess(initiateWithdrawalResponse);

            }
            catch (Exception exception)
            {
                _logger.LogError("Error while initiating withdrawal: ", exception);
                return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);
            }

        }

        public async Task<BaseResponse<ApproveWithdrawalRequest>> ApproveWithdrawalAsync(string username, ApproveWithdrawalRequest request)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(username);

                if (existingUser == null)
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var existingTransaction = await _unitOfWork.Transactions.GetByIdAsync(request.TransactionId);

                if (existingTransaction == null)
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WITHDRAWAL_REQUEST_NOT_FOUND, ResponseStatusCodes.Not_Found);
                }

                if(existingTransaction.TransactionStatus == TransactionStatus.Approved)    // Ensures that no user can spend the same funds more than once.
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WITHDRAWAL_HAS_BEEN_PROCESSED, ResponseStatusCodes.ALREADY_EXISTS);
                }

                var existingWallet = await _unitOfWork.Wallets.GetByUserIdAsync(existingUser.Id);

                if (existingWallet == null)
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var hasRequestBeenAltered = Utils.WithdrawalRequestHasBeenAltered(request, existingTransaction);

                if (hasRequestBeenAltered)
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.WITHDRAWAL_REQUEST_HAS_BEEN_ALTERED, ResponseStatusCodes.BAD_REQUEST);
                }

                var currentBalance = existingWallet.Balance;

                if (currentBalance < request.Amount)      // Ensures that the balance of wallat cannot be negative
                {
                    return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.INSUFFICIENT_FUNDS, ResponseStatusCodes.BAD_REQUEST);
                }

                // Begin Transaction
                _unitOfWork.BeginTransaction();

                // Update wallet balance
                existingWallet.Balance = currentBalance - request.Amount;
                existingWallet.LastModified = DateTime.UtcNow;
                _unitOfWork.Wallets.Update(existingWallet);

                // Update transaction status
                existingTransaction.TransactionStatus = TransactionStatus.Approved;
                existingTransaction.LastModified = DateTime.UtcNow;
                _unitOfWork.Transactions.Update(existingTransaction);

                // Post to ledger
                var newLedger = new Ledger
                {
                    TransactionId = existingTransaction.Id,
                    CurrentBalance = currentBalance,
                    TransactionType = WalletFlowType.Debit,
                    Debit = existingTransaction.Amount,
                    Credit = 0,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };
                _unitOfWork.Ledgers.Add(newLedger);
                _unitOfWork.Commit();

                return BaseResponse<ApproveWithdrawalRequest>.WithSuccess(request);

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError("Error while approving withdrawal: ", ex.Message);
                return BaseResponse<ApproveWithdrawalRequest>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
