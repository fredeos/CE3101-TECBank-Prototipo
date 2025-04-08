

namespace tecbank.models{
    /// <summary>
    /// Model class for a bank loan in the database
    /// </summary>
    public class BankLoan{
        public int id {get; set;}
        public int lapse {get; set;}
        public DateTime request_date {get; set;}
        public float interest_rate {get; set;}
        public float balance {get; set;}
        public float total {get; set;}
        public int state {get; set;}
        public int currency_id {get; set;}
        public int client_id {get; set;}
        public int adviser_id {get; set;}
    }
}