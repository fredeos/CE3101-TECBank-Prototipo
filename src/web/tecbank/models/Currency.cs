using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary> 
    /// Model class for a bank currency in the database
    /// </summary>
    /// id, name, usd_exchange
    [XmlRoot("value")]
    public class Currency {
        [XmlElement("id")]
        public int id {get; set;}

        //public int id {get; set;}

        [XmlElement("name")]
        public String name {get; set;} = string.Empty; // Warning solution

        //public String name {get; set;} = string.Empty; // Warning solution

        [XmlElement("usd_exchange")]
        public float usd_exchange{get; set;}

        //public float usd_exchange {get; set;}
    }
}