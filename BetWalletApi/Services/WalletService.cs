using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;
using BetWalletApi.Helpers;
using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Constants;
using BetWalletApi.Models.Common.Enums;
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
                var user = await _unitOfWork.Users.FindByUsernameAsync(request.Username);

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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError("Error while creating wallet: ", ex.Message);
                return BaseResponse<CreateWalletResponse>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);

            }

        }

        public async Task<BaseResponse<FundWalletResponse>> FundWalletAsync(FundWalletRequest request)
        {
            if(!Utils.IsFundTransactionType(request.TransactionType))
            {
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INVALID_TRANSACTION_TYPE, ResponseStatusCodes.BAD_REQUEST);
            }

            if(request.Amount < 0)
            {
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INVALID_AMOUNT, ResponseStatusCodes.BAD_REQUEST);
            }

            try
            {
                var existingUser = await _unitOfWork.Users.FindByUsernameAsync(request.Username);

                if(existingUser == null)
                {
                    return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.WALLET_DO_NOT_EXIST, ResponseStatusCodes.Not_Found);
                }

                var newTransaction = new Transaction
                {
                    UserId = existingUser.Id,
                    TransactionType = Utils.ConvertStringToTransactionType(request.TransactionType),
                    Amount = request.Amount,
                    TransactionReference = request.TransactionReference,
                    PostToLedger = PostTransactionToLedger.NotPostedToLedger,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(newTransaction);
                _unitOfWork.Save();

                var fundWalletResponse = new FundWalletResponse
                {
                    Amount = request.Amount,
                    TransactionId = newTransaction.Id,
                    TransactionReference = request.TransactionReference,
                    TransactionType = request.TransactionType,
                    Username = request.Username
                };

                return BaseResponse<FundWalletResponse>.WithSuccess(fundWalletResponse);

            } catch(Exception ex)
            {
                _logger.LogError("Error while funding wallet: ", ex.Message);
                return BaseResponse<FundWalletResponse>.WithError(ErrorMessages.INTERNAL_ERROR_MESSAGE, ResponseStatusCodes.INTERNAL_SERVER_ERROR);
            }


        }

       
    }
}
