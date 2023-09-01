using System.Globalization;
using GICExam.BusinessLogic;
using GICExam.Model;

namespace GICExam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isInitialized = false;
            while(true)
            {
                if (isInitialized)
                {
                    Console.WriteLine("\n\nIs there anything else you'd like to do?");
                } 
                else
                {
                    Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
                }
                
                Console.WriteLine("[I]nput transactions");
                Console.WriteLine("[D]efine interest rules");
                Console.WriteLine("[P]rint statement");
                Console.WriteLine("[Q]uit");
                Console.Write("> ");

                isInitialized = true;

                string choice = Console.ReadLine();

                switch (choice.ToUpper())
                {
                    case "I":
                        InputTransaction();
                        break;
                    case "D":
                        DefineInterestRules();
                        break;
                    case "P":
                        PrintStatement();
                        break;
                    case "Q":
                        Console.Write("\nThank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }
        }

        // [I]nput transactions
        static void InputTransaction()
        {
            TransactionBLL transactionBLL = new TransactionBLL();

            while(true)
            {
                Console.Write("\n\nPlease enter transaction details in <Date>|<Account>|<Type>|<Amount> format\n(or enter blank to go back to main menu):");
                string transactionInput = Console.ReadLine();
                string[] transactionDetails = transactionInput.Split('|');

                try
                {
                    (string errorMessage, List<Transaction> transactions) = transactionBLL.InsertTransaction(transactionDetails);
                    Console.Write(errorMessage);

                    if(string.IsNullOrEmpty(errorMessage))
                    {
                        Console.WriteLine(string.Format("Account: {0}", transactionDetails[1]));
                        Console.WriteLine("Date     | Txn Id      |Type |Amount");

                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine((string.Format("{0} | {1} | {2}   | {3}",
                                transaction.Date.ToString("yyyyMMdd")
                                , transaction.Id, transaction.TransactionType, transaction.Amount)));
                        }
                        break;
                    }

                }
                catch(Exception ex)
                {
                    Console.Write("\nERROR: Invalid Input");
                    //throw;
                }
                
            }

        }

        // [D]efine interest rules
        static void DefineInterestRules()
        {
            InterestRuleBLL interestRuleBLL = new InterestRuleBLL();

            while (true)
            {
                Console.Write("\n\nPlease enter interest rules details in <Date>|<RuleId>|<Rate in %> format\n(or enter blank to go back to main menu):");
                string interestRuleInput = Console.ReadLine();

                string[] interestRuleDetails = interestRuleInput.Split('|');

                try
                {
                    (string errorMessage, List<InterestRule> interestRules) = interestRuleBLL.AddInterestRule(interestRuleDetails);
                    Console.Write(errorMessage);

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        Console.WriteLine(string.Format("Interest rules:"));
                        Console.WriteLine("Date     | Rule Id       |Rate");

                        foreach (var interestRule in interestRules)
                        {
                            Console.WriteLine((string.Format("{0} | {1}     | {2}   ",
                                interestRule.Date.ToString("yyyyMMdd")
                                , interestRule.Id, interestRule.Rate)));
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("\nERROR: Invalid Input");
                }
            }
        }

        //[P]rint statement
        static void PrintStatement()
        {
            TransactionBLL transactionBLL = new TransactionBLL();

            while (true)
            {
                Console.Write("\n\nPlease enter account and month to generate the statement <Account>|<Month>\n(or enter blank to go back to main menu):");
                string printStatementInput = Console.ReadLine();

                string[] printStatementDetails = printStatementInput.Split('|');

                try
                {
                    List<Transaction> transactions= transactionBLL.GetTransactions(printStatementDetails);

                    Console.WriteLine(string.Format("Account: {0}", printStatementDetails[0]));
                    Console.WriteLine("Date     | Txn Id      |Type |Amount    | Balance");

                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine((string.Format("{0} | {1} | {2}   | {3}   |{4}",
                            transaction.Date.ToString("yyyyMMdd")
                            , transaction.Id, transaction.TransactionType, transaction.Amount, 10)));
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.Write("\nERROR: Invalid Input");
                }
            }

        }
    }
}