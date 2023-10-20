using BetWalletApi.Models.Common.Enums;

namespace BetWalletApi.Helpers
{
    public class Utils
    {
        public static bool IsWithdrawTransactionType(string transactionType)
        {
            TransactionType enumTransactionType;

            if (Enum.TryParse(transactionType, true, out enumTransactionType))
            {
                if (enumTransactionType == TransactionType.Bet || 
                    enumTransactionType == TransactionType.Withdrawal)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFundTransactionType(string transactionType)
        {
            TransactionType enumTransactionType;

            if (Enum.TryParse(transactionType, true, out enumTransactionType))
            {
                if (enumTransactionType == TransactionType.Winning ||
                    enumTransactionType == TransactionType.Deposit || 
                    enumTransactionType == TransactionType.Bonus)
                {
                    return true;
                }
            }

            return false;
        }

        public static TransactionType ConvertStringToTransactionType(string transactionType)
        {
            return Enum.Parse<TransactionType>(transactionType, true);
        }
    }
}
