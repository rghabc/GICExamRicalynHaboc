using GICExam.Model;

namespace GICExam.DataAccess
{
    public class DataLoader
    {
        private static List<Transaction> transactions = new List<Transaction>
        {
            new Transaction { Id = "20230505-01", Account = "AC001", TransactionType = 'D', Amount = 100.00m, Date = new DateTime(2023, 05, 05) },
            new Transaction { Id = "20230601-01", Account = "AC001", TransactionType = 'D', Amount = 150.00m, Date = new DateTime(2023, 06, 01) },
            new Transaction { Id = "20230626-01", Account = "AC001", TransactionType = 'W', Amount = 20.00m, Date = new DateTime(2023, 06, 26) },
            new Transaction { Id = "20230626-02", Account = "AC001", TransactionType = 'W', Amount = 100.00m, Date = new DateTime(2023, 06, 26) },
        };

        private static List<InterestRule> interestRules = new List<InterestRule>
        {
            new InterestRule { Id = "RULE01", Rate = 1.95m, Date = new DateTime(2023, 01, 01) },
            new InterestRule { Id = "RULE02", Rate = 1.90m, Date = new DateTime(2023, 05, 20) },
            new InterestRule { Id = "RULE03", Rate = 2.20m, Date = new DateTime(2023, 06, 16) },
        };

        public List<Transaction> Transactions { get { return transactions; } }

        public List<InterestRule> InterestRules { get {  return interestRules; } }

        public List<Transaction> GetTransactionsByAccount(string account)
        {
            return transactions.Where(transaction => transaction.Account.ToLower()  == account.ToLower()).ToList();
        }

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public void AddInterestRule(InterestRule interestRule)
        {
            interestRules.Add(interestRule);
        }

        public void RemoveInterestRuleByDate(DateTime date)
        {
            var interestRuleToRemove = interestRules.SingleOrDefault(interestRule => interestRule.Date == date);
            if (interestRuleToRemove != null) {
                interestRules.Remove(interestRuleToRemove);
            }
        }

        public string CreateTransactionId(List<Transaction> existingTransactions, DateTime date)
        {
            var transactionDates = existingTransactions.Where(transaction => transaction.Date == date && !string.IsNullOrEmpty(transaction.Id)).ToArray();

            // there is no existing transaction date
            if (transactionDates.Length == 0)
            {
                return $"{date.ToString("yyyyMMdd")}-01";
            }

            // parse latest value
            // sort array
            Array.Sort(transactionDates);
            string latestTransactionDate = transactionDates[transactionDates.Length - 1].Id;
            string[] words = latestTransactionDate.Split('-');
            var id = Int32.Parse(words[1]) + 1;
            var paddedId = id.ToString().PadLeft(2, '0');


            return $"{date.ToString("yyyyMMdd")}-{paddedId}";
        }
    }
}