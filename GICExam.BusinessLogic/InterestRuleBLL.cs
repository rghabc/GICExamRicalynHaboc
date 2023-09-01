using GICExam.Model;
using GICExam.DataAccess;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace GICExam.BusinessLogic
{
    public class InterestRuleBLL
    {
        private DataLoader dataLoader;

        public InterestRuleBLL()
        {
            dataLoader = new DataLoader();
        }

        public (string errorMessage, List<InterestRule> interestRules) AddInterestRule(string[] interestRuleDetails)
        {
            InterestRule newInterestRule = new InterestRule();

            DateTime interestRuleDate = new DateTime();

            // Date should be in YYYYMMdd format
            if (!DateTime.TryParseExact(interestRuleDetails[0], "yyyyMMdd", CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out interestRuleDate))
            {
                return (ErrorMessages.INVALID_DATE_FORMAT, dataLoader.InterestRules);
            }

            newInterestRule = new InterestRule()
            {
                Id = interestRuleDetails[1],
                Date = interestRuleDate,
                Rate = Convert.ToDecimal(interestRuleDetails[2])
            };

            // Interest rate should be greater than 0 and less than 100
            // Amount must be greater than zero
            if (newInterestRule.Rate <= 0 || newInterestRule.Rate > 100)
            {
                return (ErrorMessages.RATE_GREATERTHAN_ZERO_LESS_ONEHUNDRED, dataLoader.InterestRules);
            }

            //TODO: If there's any existing rules on the same day, the latest one is kept

            dataLoader.RemoveInterestRuleByDate(newInterestRule.Date);

            dataLoader.AddInterestRule(newInterestRule);

            return ("", dataLoader.InterestRules);
        }

    }
}