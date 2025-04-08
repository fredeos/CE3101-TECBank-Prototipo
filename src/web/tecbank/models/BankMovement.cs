namespace tecbank.models {
    /// <summary>
    /// Model class for bank movements saved in the database
    /// </summary>
    /// id, total_transfer, date, description, type, card_id, account_id, currency_id
    public class BankMovement {
        public String id {get; set;}
        public float total_transfer {get; set;}
        public DateTime date {get; set;}
        public String description {get; set;}
        public int type {get; set;}
        public int card_id {get; set;}
        public String account_id {get; set;}
        public int currency_id {get; set;}
    }
}