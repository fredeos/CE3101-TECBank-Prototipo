
namespace tecbank.models{
    /// <summary>
    /// Model class for a card give by the bank in the database
    /// </summary>
    /// card_num, type, cvc, balance, account_id
    public class BankCard {
        public int card_num {get; set;}
        public int type {get; set;}
        public int cvc {get; set;}
        public int balance {get; set;}
        public String account_id {get; set;}
    }
}