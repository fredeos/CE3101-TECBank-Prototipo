using System.Xml.Serialization;

namespace tecbank.models {
    /// <summary>
    /// Model class for loan payments schedule in the database
    /// </summary>
    /// loan_id, movement_id, type, date, total, card_id
    [XmlRoot("value")]
    public class LoanPayment {
        [XmlElement("id")]
        public String id {get; set;}
        
        [XmlElement("loan_id")]
        public int loan_id {get; set;}
        
        [XmlElement("movement_id")]
        public String movement_id {get; set;}
        
        [XmlElement("type")]
        public int type {get; set;}
        
        [XmlElement("date")]
        public DateTime date {get; set;}
        
        [XmlElement("total")]
        public float total {get; set;}
        
        [XmlElement("state")]
        public int state {get; set;}
    }
}