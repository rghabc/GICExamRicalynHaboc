using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GICExam.Model;

namespace GICExam.BusinessLogic.Test
{
    [TestClass]
    public  class InterestRuleBLLTest
    {
        InterestRuleBLL bll = new InterestRuleBLL();

        [TestMethod]
        public void When_Date_IsInvalid_Format_Rules_NoChange()
        {
            //Arrange
            string[] interestRuleDetails = { "2023050Y", "RULE1", "1.95"};

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(4, interestRules.Count);
            Assert.AreEqual(ErrorMessages.INVALID_DATE_FORMAT, errorMessage);

        }

        [TestMethod]
        public void When_Rate_IsEqual_Zero_Rules_NoChange()
        {
            //Arrange
            string[] interestRuleDetails = { "20230901", "RULE1", "0" };

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(4, interestRules.Count);
            Assert.AreEqual(ErrorMessages.RATE_GREATERTHAN_ZERO_LESS_ONEHUNDRED, errorMessage);

        }

        [TestMethod]
        public void When_Rate_IsLessThan_Zero_Rules_NoChange()
        {
            //Arrange
            string[] interestRuleDetails = { "20230901", "RULE1", "-1" };

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(4, interestRules.Count);
            Assert.AreEqual(ErrorMessages.RATE_GREATERTHAN_ZERO_LESS_ONEHUNDRED, errorMessage);
        }

        [TestMethod]
        public void When_Rate_IsGreaterThan_OneHundred_Rules_NoChange()
        {
            //Arrange
            string[] interestRuleDetails = { "20230901", "RULE1", "101" };

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(4, interestRules.Count);
            Assert.AreEqual(ErrorMessages.RATE_GREATERTHAN_ZERO_LESS_ONEHUNDRED, errorMessage);
        }

        [TestMethod]
        public void When_Rule_Overrides_Same_Day()
        {
            //Arrange
            string[] interestRuleDetails = { "20230616", "RULE5", "50" };

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.AreEqual(4, interestRules.Count);
            Assert.AreEqual(string.Empty, errorMessage);
        }

        [TestMethod]
        public void When_Add_New_ValidRule_Successfully_Added()
        {
            //Arrange
            string[] interestRuleDetails = { "20230915", "RULE5", "50" };

            //Assert
            (string errorMessage, List<InterestRule> interestRules) = bll.AddInterestRule(interestRuleDetails);

            //Act
            Assert.AreEqual(errorMessage, string.Empty);
            Assert.IsTrue(interestRules.Single(x=> x.Date.ToString("yyyyMMdd") == "20230915" && x.Id == "RULE5").Rate == 50);
        }
    }
}
