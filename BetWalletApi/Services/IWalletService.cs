﻿using BetWalletApi.DTOs.Requests;
using BetWalletApi.DTOs.Responses;

namespace BetWalletApi.Services
{
    public interface IWalletService
    {
        Task<BaseResponse<CreateWalletResponse>> CreateWalletAsync(CreateWalletRequest request);
        Task<BaseResponse<FundWalletResponse>> FundWalletAsync(FundWalletRequest request);
    }
}