

namespace tecbank.models{
    /// <summary> 
    /// Model class for a bank account in the database
    /// </summary>
    /// id, type, balance, description, currency_id, client_id
    public class BankAccount{
        public String id {get; set;}
        public int type {get; set;}
        public int balance {get; set;}
        public String description {get; set;}
        public int currency_id {get; set;}
        public int client_id {get; set;}
    }
}