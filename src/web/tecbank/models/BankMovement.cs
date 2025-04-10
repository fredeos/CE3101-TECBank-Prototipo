using System.Xml.Serialization;

namespace tecbank.models {
    /// <summary>
    /// Model class for bank movements saved in the database
    /// </summary>
    /// id, total_transfer, date, description, type, card_id, account_id, currency_id
    [XmlRoot("value")]
    public class BankMovement {
        [XmlElement("id")]
        public String id {get; set;}
        
        [XmlElement("total_transfer")]
        public float total_transfer {get; set;}
        
        [XmlElement("date")]
        public DateTime date {get; set;}
        
        [XmlElement("description")]
        public String description {get; set;}
        
        [XmlElement("type")]
        public int type {get; set;}
        
        [XmlElement("card_id")]
        public int card_id {get; set;}
        
        [XmlElement("account_id")]
        public String account_id {get; set;}
        
        [XmlElement("currency_id")]
        public int currency_id {get; set;}
    }
}