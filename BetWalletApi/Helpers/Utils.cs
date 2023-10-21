using BetWalletApi.DTOs.Requests;
using BetWalletApi.Models.Common.Enums;
using BetWalletApi.Models.Transactions;

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

        public static bool WithdrawalRequestHasBeenAltered(ApproveWithdrawalRequest withdrawalRequest, Transaction withdrawalTransaction)
        {
            // Alternatively, checksum approach could be used.
            // Encrypt the amount and transaction type during initiate request and return to the client
            // Regenerate the checksum during approval and compare with the value returned checksum in the approval request.

            if(!Decimal.Equals(withdrawalRequest.Amount, withdrawalTransaction.Amount))
            {
                return true;
            }

            TransactionType requestTransactionType = ConvertStringToTransactionType(withdrawalRequest.TransactionType);

            if(requestTransactionType != withdrawalTransaction.TransactionType)
            {
                return true;
            }

            return false;
        }
    }
}
