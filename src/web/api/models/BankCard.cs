
using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for a card give by the bank in the database
    /// </summary>
    /// card_num, type, cvc, balance, account_id
    [XmlRoot("value")]
    public class BankCard {
        [XmlElement("card_num")]
        public int card_num {get; set;}

        [XmlElement("type")]
        public int type {get; set;}

        [XmlElement("cvc")]
        public int cvc {get; set;}

        [XmlElement("balance")]
        public double balance {get; set;}

        [XmlElement("account_id")]
        public String account_id {get; set;}

        [XmlElement("rem_state")]
        public int removed {get; set;}
    }
}