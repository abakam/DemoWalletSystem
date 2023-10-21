using System.ComponentModel;

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

    public enum WalletFlowType
    {
        [Description("Credit")]
        Credit,
        [Description("Debit")]
        Debit

    }

    public enum TransactionStatus
    {
        [Description("Initiated")]
        Initiated,
        [Description("Approved")]
        Approved
    }

}
