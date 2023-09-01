namespace GICExam.Model
{
    public class Transaction
    {
        public string? Id { get; set; }
        public string Account { get; set; }
        public char TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}