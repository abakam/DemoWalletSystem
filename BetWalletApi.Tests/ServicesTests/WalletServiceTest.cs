using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;
using BetWalletApi.Models.Common;
using BetWalletApi.Models.Common.Constants;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using BetWalletApi.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace BetWalletApi.Tests.ServicesTests
{
    public class WalletServiceTest
    {

        private readonly Mock<IUnitOfWork> _unitOfWork;
        
        private readonly ILogger<WalletService> _logger;

        public WalletServiceTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new LoggerFactory().CreateLogger<WalletService>();
        }

        [Fact]
        public async Task CreateWalletAsync_BaseResponseWithSuccess_NewUser()
        {
            // Arrange
            string username = "abakam";
            string firstName = "Abraham";
            string lastName = "Akam";
            string email = "abraham";

            var createWalletRequest = new CreateWalletRequest
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            User nullUser = null;

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Returns(nullUser);
            _unitOfWork.Setup(x => x.Wallets.Add(It.IsAny<Wallet>())).Verifiable();

            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualCreateWalletResponse = await walletService.CreateWalletAsync(createWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<CreateWalletResponse>>(actualCreateWalletResponse);
            Assert.True(actualCreateWalletResponse.Success);
            Assert.True(String.IsNullOrWhiteSpace(actualCreateWalletResponse.Message));
            Assert.True(String.Equals(username, actualCreateWalletResponse.Result.Username));
            Assert.True(String.Equals(firstName, actualCreateWalletResponse.Result.FirstName));
            Assert.True(String.Equals(email, actualCreateWalletResponse.Result.Email));
            Assert.True(String.Equals(lastName, actualCreateWalletResponse.Result.LastName));
            Assert.NotEqual(Guid.Empty, actualCreateWalletResponse.Result.WalletId);

        }

        [Fact]
        public async Task CreateWalletAsync_BaseResponseWithError_UsernameExists()
        {
            // Arrange
            string username = "abakam";
            string firstName = "Abraham";
            string lastName = "Akam";
            string email = "abraham";

            var createWalletRequest = new CreateWalletRequest
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Returns(new User { });

            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualCreateWalletResponse = await walletService.CreateWalletAsync(createWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<CreateWalletResponse>>(actualCreateWalletResponse);
            Assert.False(actualCreateWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.USERNAME_ALREADY_EXISTS, actualCreateWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.ALREADY_EXISTS, actualCreateWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task CreateWalletAsync_BaseResponseWithError_InternalServerError()
        {
            // Arrange
            string username = "abakam";
            string firstName = "Abraham";
            string lastName = "Akam";
            string email = "abraham";

            var createWalletRequest = new CreateWalletRequest
            {
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Throws(new Exception());

            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualCreateWalletResponse = await walletService.CreateWalletAsync(createWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<CreateWalletResponse>>(actualCreateWalletResponse);
            Assert.False(actualCreateWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.INTERNAL_ERROR_MESSAGE, actualCreateWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.INTERNAL_SERVER_ERROR, actualCreateWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithSuccess_ValidInputs()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = 5000,
                TransactionType = "Deposit"
            };

            string username = "abakam";
            Guid userId = Guid.NewGuid();

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Returns(new User { Id = userId });
            _unitOfWork.Setup(x => x.Wallets.GetByUserIdAsync(userId).Result).Returns(new Wallet { });
            _unitOfWork.Setup(x => x.Transactions.Add(It.IsAny<Transaction>())).Verifiable();

            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.True(actualFundWalletResponse.Success);
            Assert.True(String.IsNullOrWhiteSpace(actualFundWalletResponse.Message));
            Assert.True(String.Equals(username, actualFundWalletResponse.Result.Username));
            Assert.True(String.Equals(fundWalletRequest.TransactionType, actualFundWalletResponse.Result.TransactionType));
            Assert.NotEqual(Guid.Empty, actualFundWalletResponse.Result.TransactionId);
            Assert.Equal(fundWalletRequest.Amount, actualFundWalletResponse.Result.Amount, 2);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithError_InvalidTransactionTypeInput()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = 5000,
                TransactionType = "Bet"
            };

            string username = "abakam";
         

            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.False(actualFundWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.INVALID_TRANSACTION_TYPE, actualFundWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.BAD_REQUEST, actualFundWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithError_NegativeAmountInput()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = -5000,
                TransactionType = "Deposit"
            };

            string username = "abakam";


            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.False(actualFundWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.INVALID_AMOUNT, actualFundWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.BAD_REQUEST, actualFundWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithError_UserDoNotExists()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = 5000,
                TransactionType = "Deposit"
            };

            string username = "abakam";
            User nullUser = null;

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Returns(nullUser);
            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.False(actualFundWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.WALLET_DO_NOT_EXIST, actualFundWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.Not_Found, actualFundWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithError_UserDoNotHaveWallet()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = 5000,
                TransactionType = "Deposit"
            };

            string username = "abakam";
            Wallet nullWallet = null;

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Returns(new User { Id = Guid.NewGuid()});
            _unitOfWork.Setup(x => x.Wallets.GetByUserIdAsync(Guid.NewGuid()).Result).Returns(nullWallet);
            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.False(actualFundWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.WALLET_DO_NOT_EXIST, actualFundWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.Not_Found, actualFundWalletResponse.ErrorCode);

        }

        [Fact]
        public async Task FundWalletAsync_BaseResponseWithError_InternalServerError()
        {
            // Arrange
            var fundWalletRequest = new FundWalletRequest
            {
                Amount = 5000,
                TransactionType = "Deposit"
            };

            string username = "abakam";

            _unitOfWork.Setup(x => x.Users.GetByUsernameAsync(username).Result).Throws(new Exception());
            var walletService = new WalletService(_unitOfWork.Object, _logger);

            // Act
            var actualFundWalletResponse = await walletService.FundWalletAsync(username, fundWalletRequest);

            // Assert
            Assert.IsType<BaseResponse<FundWalletResponse>>(actualFundWalletResponse);
            Assert.False(actualFundWalletResponse.Success);
            Assert.True(String.Equals(ErrorMessages.INTERNAL_ERROR_MESSAGE, actualFundWalletResponse.Message));
            Assert.Equal(ResponseStatusCodes.INTERNAL_SERVER_ERROR, actualFundWalletResponse.ErrorCode);

        }

    }
}
