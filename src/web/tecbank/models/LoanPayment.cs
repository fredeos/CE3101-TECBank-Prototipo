namespace tecbank.models {
    /// <summary>
    /// Model class for loan payments schedule in the database
    /// </summary>
    /// loan_id, movement_id, type, date, total, card_id
    public class LoanPayment {
        public String id {get; set;}
        public int loan_id {get; set;}
        public String movement_id {get; set;}
        public int type {get; set;}
        public DateTime date {get; set;}
        public float total {get; set;}
    }
}