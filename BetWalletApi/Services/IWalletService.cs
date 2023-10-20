using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;

namespace BetWalletApi.Services
{
    public interface IWalletService
    {
        Task<BaseResponse<CreateWalletResponse>> CreateWallet(CreateWalletRequest request);
    }
}
