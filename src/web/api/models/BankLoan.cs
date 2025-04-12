

using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for a bank loan in the database
    /// </summary>
    [XmlRoot("value")]
    public class BankLoan{
        [XmlElement("id")]
        public int id {get; set;}
        
        [XmlElement("lapse")]
        public int lapse {get; set;}

        [XmlElement("request_date")]
        public DateTime request_date {get; set;}

        [XmlElement("interest_rate")]
        public float interest_rate {get; set;}

        [XmlElement("balance")]
        public float balance {get; set;}

        [XmlElement("total")]
        public float total {get; set;}

        [XmlElement("state")]
        public int state {get; set;}

        [XmlElement("currency_id")]
        public int currency_id {get; set;}

        [XmlElement("client_id")]
        public int client_id {get; set;}

        [XmlElement("adviser_id")]
        public int adviser_id {get; set;}

        [XmlElement("rem_state")]
        public int removed {get; set;}
    }
}