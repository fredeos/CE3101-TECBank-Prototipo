namespace tecbank.models {
    /// <summary>
    /// Model class for loan payments schedule in the database
    /// </summary>
    /// loan_id, movement_id, type, date, total, card_id
    public class LoanPayment {
        public int loan_id {get; set;}
        public String movement_id {get; set;}
        public int type {get; set;}
        public DateTime date {get; set;}
        public int total {get; set;}
        public int card_id {get; set;}
    }
}