namespace tecbank.models
{
    public class ClientLoanReportDTO
    {
        public string FullName { get; set; }
        public int LoanId { get; set; }
        public DateTime RequestDate { get; set; }
        public float RemainingBalance { get; set; }
        public float TotalAmount { get; set; }
        public float InterestRate { get; set; }
        public List<OverduePaymentInfo> OverduePayments { get; set; } = new List<OverduePaymentInfo>();
    }

    public class OverduePaymentInfo
    {
        public string PaymentId { get; set; }
        public DateTime DueDate { get; set; }
        public float Amount { get; set; }
    }
}