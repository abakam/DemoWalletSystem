using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;
using BetWalletApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetWalletApi.Controllers
{
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

            var createWalletResponse = await _walletService.CreateWallet(createWallet);

            if(createWalletResponse.Success)
            { 
                return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateWalletResponse> { Data = createWalletResponse.Result });
            }

            return StatusCode(createWalletResponse.ErrorCode, new ApiResponse<string> { ErrorMessage = createWalletResponse.Message });
        }

        [HttpPost("{username}/deposits")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ApiResponse<FundWalletRequest>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FundWalletAsync([FromBody] FundWalletRequest fundWallet)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{username}/withdrawals")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<FundWalletRequest>))]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DebitWalletAsync([FromBody] FundWalletRequest debitWallet)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof (ApiResponse<FundWalletRequest>))]    // Replace with appropriate type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWalletDetailsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
