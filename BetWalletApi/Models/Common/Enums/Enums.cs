﻿using System.ComponentModel;

namespace BetWalletApi.Models.Common.Enums
{
    public enum TransactionType
    {
        [Description("Winning")]
        Winning,
        [Description("Deposit")]
        Deposit,
        [Description("Bonus")]
        Bonus,
        [Description("Bet")]
        Bet,
        [Description("Withdrawal")]
        Withdrawal
    }

    public enum WalletFlow
    {
        [Description("Credit")]
        Credit,
        [Description("Debit")]
        Debit

    }
    
}
