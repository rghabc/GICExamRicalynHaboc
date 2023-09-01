using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GICExam.BusinessLogic
{
    public class ErrorMessages
    {
        public const string INVALID_DATE_FORMAT = "ERROR: Invalid  Date";
        public const string INVALID_TRANSACTION_TYPE = "ERROR: Invalid Transaction Type";
        public const string TRANSACTION_AMOUNT_ZERO = "ERROR: Amount Should be Greater than 0";
        public const string INVALID_TRANSACTION_AMOUNT_DECIMALS = "ERROR: Amount Should only have 2 decimal places";
        public const string NEWUSER_ONLY_DEPOSIT = "ERROR: New users can only deposit";
        public const string USER_INSUFFICIENT_BALANCE = "ERROR: Insufficient Balance";

        public const string RATE_GREATERTHAN_ZERO_LESS_ONEHUNDRED = "ERROR: Amount Should be Greater than 0 but less than 100";
    }
}
