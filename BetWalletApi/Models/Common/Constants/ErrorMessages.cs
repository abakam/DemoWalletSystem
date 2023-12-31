﻿namespace BetWalletApi.Models.Common.Constants
{
    public static class ErrorMessages
    {
        public static readonly string INTERNAL_ERROR_MESSAGE = "An error occurred while processing your request.";
        public static readonly string WALLET_DO_NOT_EXIST = "Wallet do not exists.";
        public static readonly string USERNAME_ALREADY_EXISTS = "Username already exist.";
        public static readonly string INVALID_TRANSACTION_TYPE = "Invalid Transaction.";
        public static readonly string INVALID_AMOUNT = "Invalid Amount.";
        public static readonly string WITHDRAWAL_REQUEST_NOT_FOUND = "Withdrawal request not found.";
        public static readonly string WITHDRAWAL_REQUEST_HAS_BEEN_ALTERED = "Withdrawal request has been altered.";
        public static readonly string INSUFFICIENT_FUNDS = "Insufficcient funds.";
        public static readonly string WITHDRAWAL_HAS_BEEN_PROCESSED = "Withdrawal has already been processed.";
    }
}
