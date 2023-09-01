using GICExam.Model;

namespace GICExam.BusinessLogic.Test
{
    [TestClass]
    public class TransactionBLLTest
    {
        TransactionBLL bll = new TransactionBLL();

        [TestMethod]
        public void When_Date_IsInvalid_Format_Transactions_NoChange()
        {
            //Arrange
            string[] transactionDetails = { "2023050Y", "AC001", "D", "0" };

            //Assert
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(5, transactions.Count);
            Assert.AreEqual(ErrorMessages.INVALID_DATE_FORMAT, errorMessage);

        }

        public void When_TransactionType_IsInvalid_Transactions_NoChange()
        {
            //Arrange
            string[] transactionDetails = { "20230901", "AC001", "f", "1.20" };

            //Assert
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(0, transactions.Count);
            Assert.AreEqual(ErrorMessages.INVALID_TRANSACTION_TYPE, errorMessage);

        }

        [TestMethod]
        public void When_Amount_IsEqual_Zero_Transactions_NoChange()
        {
            //Arrange
            string[] transactionDetails = { "20230901", "AC001", "D", "0" };

            //Assert
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(5, transactions.Count);
            Assert.AreEqual(ErrorMessages.TRANSACTION_AMOUNT_ZERO, errorMessage);

        }

        [TestMethod]
        public void When_Amount_IsLessThan_Zero_Transactions_NoChange()
        {
            //Arrange
            string[] transactionDetails = { "20230901", "AC001", "D", "-1" };

            //Assert
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(5, transactions.Count);
            Assert.AreEqual(ErrorMessages.TRANSACTION_AMOUNT_ZERO, errorMessage);
        }

        [TestMethod]
        public void When_NewUser_Withdraw_Transactions_NoChange()
        {
            //Arrange
            string[] transactionDetails = { "20230901", "TESTDUMMY", "W", "500" };

            //Assert
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(5, transactions.Count);
            Assert.AreEqual(ErrorMessages.NEWUSER_ONLY_DEPOSIT, errorMessage);

        }

        [TestMethod]
        public void When_User_InsufficientBalance_Transactions_NoChange()
        {
           //Arrange
            string[] transactionDetails = { "20230901", "AC001", "W", "99999999" };

            //Assert 
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(5, transactions.Count);
            Assert.AreEqual(ErrorMessages.USER_INSUFFICIENT_BALANCE, errorMessage);

        }

        [TestMethod]
        public void When_Add_New_ValidTransaction_Successfully_Added()
        {
            //Arrange
            string[] transactionDetails = { "20230901", "AC001", "W", "50" };

            //Assert 
            (string errorMessage, List<Transaction> transactions) = bll.InsertTransaction(transactionDetails);

            //Act
            Assert.AreEqual(errorMessage, string.Empty);
            Assert.IsTrue(transactions.Single(x => x.Date.ToString("yyyyMMdd") == "20230901" && x.Account == "AC001").Amount == 50);
        }
    }
}