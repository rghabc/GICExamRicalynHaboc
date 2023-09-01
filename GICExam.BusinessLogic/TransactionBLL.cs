using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GICExam.DataAccess;
using GICExam.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GICExam.BusinessLogic
{
    public class TransactionBLL
    {
        private DataLoader dataLoader;

        public TransactionBLL() 
        {
            dataLoader = new DataLoader();
        }

        public (string errorMessage, List<Transaction> transactions) InsertTransaction(string[] transactionDetails)
        {
            Transaction newTransaction = new Transaction();
            DateTime transactionDate = new DateTime();

            // Get User Transactions
            List<Transaction> userTransactions = dataLoader.GetTransactionsByAccount(transactionDetails[1]);

            // Date should be in YYYYMMdd format
            if (!DateTime.TryParseExact(transactionDetails[0], "yyyyMMdd", CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out transactionDate))
            {
                return (ErrorMessages.INVALID_DATE_FORMAT, dataLoader.Transactions);
            }

            newTransaction = new Transaction()
            {
                Id = dataLoader.CreateTransactionId(userTransactions, transactionDate),
                Account = transactionDetails[1],
                TransactionType = Convert.ToChar(transactionDetails[2]),
                Amount = Convert.ToDecimal(transactionDetails[3]),
                Date = transactionDate
            };

            // Nonexisting User can only Deposit
            if (userTransactions.Count <= 0 && !(Char.ToUpper(newTransaction.TransactionType) == 'D'))
            {
                return (ErrorMessages.NEWUSER_ONLY_DEPOSIT, dataLoader.Transactions);
            }

            // Existing User Withdraw More than Balance
            if (Char.ToUpper(newTransaction.TransactionType) == 'W' && newTransaction.Amount > GetTotalAmount(userTransactions))
            {
                return (ErrorMessages.USER_INSUFFICIENT_BALANCE, dataLoader.Transactions);
            }

            // Type is D for deposit, W for withdrawal, case insensitive
            if (!(Char.ToUpper(newTransaction.TransactionType) == 'D' || Char.ToUpper(newTransaction.TransactionType) == 'W'))
            {
                return (ErrorMessages.INVALID_TRANSACTION_TYPE, dataLoader.Transactions);
            }

            // Amount must be greater than zero
            if (newTransaction.Amount <= 0)
            {
                return (ErrorMessages.TRANSACTION_AMOUNT_ZERO, dataLoader.Transactions);
            }

            // Amount decimals are allowed up to 2 decimal places
            if (Decimal.Round(newTransaction.Amount, 2) != newTransaction.Amount)
            {
                return (ErrorMessages.INVALID_TRANSACTION_AMOUNT_DECIMALS, dataLoader.Transactions);
            }

            dataLoader.AddTransaction(newTransaction);

            return ("", dataLoader.Transactions);
        }

        public decimal GetTotalAmount(List<Transaction> transactions)
        {
            if (transactions.Count == 0)
            {
                return 0;
            }

            return transactions.Aggregate(0m, (sum, transaction) =>
            {
                switch (transaction.TransactionType)
                {
                    case 'W':
                        return sum - transaction.Amount;

                    case 'D':
                        return sum + transaction.Amount;

                    default:
                        return sum;
                }

            });
        }

        public List<Transaction> GetTransactions(string[] arguments)
        {
            List<Transaction> transactions = dataLoader.Transactions.Where(transaction => transaction.Account == arguments[0] && transaction.Date.Month == int.Parse(arguments[1])).OrderBy(transaction => transaction.Date).ToList();
            var interestRules = dataLoader.InterestRules;

            decimal totalAnnualizedInterest = 0m;
            decimal currentBalance = 0m;

            for (var i = 0; i < transactions.Count; i++)
            {
                var transaction = transactions[i];

                var interestRate = interestRules.FindLast(interestRule => interestRule.Date <= transaction.Date)?.Rate ?? 0m;

                currentBalance += Char.ToUpper(transaction.TransactionType) == 'W' ? (-transaction.Amount) : transaction.Amount;

                if (interestRate == 0)
                {
                    continue;
                }

                // get next transaction
                if (i + 1 != transactions.Count) {
                
                    var nextTransaction = transactions[i + 1];
                    var differenceInDays = (nextTransaction.Date - transaction.Date).Days;
                    totalAnnualizedInterest += currentBalance * interestRate * differenceInDays;
                }
                
            }
            var totalInterest = Math.Round((totalAnnualizedInterest / 365), 2);

            var date = transactions[0].Date;

           var interestTransaction = new Transaction()
            {
                Date = new DateTime(date.Year, date.Month, 1, 0, 0, 0).AddMonths(1).AddSeconds(-1),
                TransactionType = 'I',
                Amount = totalInterest
            };

            transactions.Add(interestTransaction);

            return transactions;
        }
    }
}
