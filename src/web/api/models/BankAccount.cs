

using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary> 
    /// Model class for a bank account in the database
    /// </summary>
    /// id, type, balance, description, currency_id, client_id
    [XmlRoot("value")]
    public class BankAccount{
        [XmlElement("id")]
        public String id {get; set;}

        [XmlElement("type")]
        public int type {get; set;}

        [XmlElement("balance")]
        public float balance {get; set;}

        [XmlElement("description")]
        public String description {get; set;}

        [XmlElement("currency_id")]
        public int currency_id {get; set;}

        [XmlElement("client_id")]
        public int client_id {get; set;}
    }
}