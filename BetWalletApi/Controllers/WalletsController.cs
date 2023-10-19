using BetWalletApi.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetWalletApi.Controllers
{
    [Route("api/v1/wallets")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest createWalletPayload)
        {
            return null;
        }
    }
}
