
using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for master data currenct types
    /// </summary>
    [XmlRoot("value")]
    public class Currency {
        [XmlElement("id")]
        public int id {get; set;}
        
        [XmlElement("name")]
        public String name {get; set;}
        
        [XmlElement("usd_exchange")]
        public float usd_exchange {get; set;}
    }
}