using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;
using BetWalletApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetWalletApi.Controllers
{
    /// <summary>
    /// API for creating wallet, funding wallet and withdrawing funds.
    /// </summary>
    [Route("api/v1/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {

        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<CreateWalletResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateWalletAsync([FromBody] CreateWalletRequest createWallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string> { ErrorMessage = "Username is required." });
            }

            var createWalletResponse = await _walletService.CreateWalletAsync(createWallet);

            if(createWalletResponse.Success)
            { 
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateWalletResponse> { Data = createWalletResponse.Result });
            }

            return StatusCode(createWalletResponse.ErrorCode, new ApiResponse<string> { ErrorMessage = createWalletResponse.Message });
        }


        [HttpPost("{username}/deposits")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ApiResponse<FundWalletResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FundWalletAsync(string username, [FromBody] FundWalletRequest fundWallet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string> { ErrorMessage = "Some inputs are invalid." });
            }

            var fundWalletResponse = await _walletService.FundWalletAsync(username, fundWallet);

            if(fundWalletResponse.Success)
            {
                return StatusCode(StatusCodes.Status202Accepted, new ApiResponse<FundWalletResponse> { Data = fundWalletResponse.Result });
            }

            return StatusCode(fundWalletResponse.ErrorCode, new ApiResponse<FundWalletResponse> { ErrorMessage = fundWalletResponse.Message });
        }

        [HttpPost("{username}/withdrawals/initiations")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ApiResponse<ApproveWithdrawalRequest>))]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiateWithdrawalAsync(string username, [FromBody] InitiateWithdrawalRequest initiateWithdrawal)
        {
            var initiateWithrawalResponse = await _walletService.InitiateWithdrawalAsync(username, initiateWithdrawal);

            if (initiateWithrawalResponse.Success)
            {
                return StatusCode(StatusCodes.Status202Accepted, new ApiResponse<ApproveWithdrawalRequest> { Data = initiateWithrawalResponse.Result });
            }
           

            return StatusCode(initiateWithrawalResponse.ErrorCode, new ApiResponse<ApproveWithdrawalRequest> { ErrorMessage = initiateWithrawalResponse.Message });
        }

        [HttpPost("{username}/withdrawals/approvals")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApproveWithdrawalRequest>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApproveWithdrawalAsync(string username, [FromBody] ApproveWithdrawalRequest approveWithdrawal)
        {
            var approveWithdrawalResponse = await _walletService.ApproveWithdrawalAsync(username, approveWithdrawal);

            if(approveWithdrawalResponse.Success)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<ApproveWithdrawalRequest> { Data = approveWithdrawalResponse.Result });
            }

            return StatusCode(approveWithdrawalResponse.ErrorCode, new ApiResponse<ApproveWithdrawalRequest> { ErrorMessage =  approveWithdrawalResponse.Message });
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof (ApiResponse<CreateWalletResponse>))]    
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWalletDetailsAsync(string username)
        {
            var walletDetails = await _walletService.GetWalletDetailsAsync(username);

            if(walletDetails.Success)
            {
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<CreateWalletResponse> { Data = walletDetails.Result });
            }

            return StatusCode(walletDetails.ErrorCode, new ApiResponse<CreateWalletResponse> { ErrorMessage = walletDetails.Message });
        }
    }
}
