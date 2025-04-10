
using System.Xml.Serialization;

namespace tecbank.models {
    /// <summary>
    /// Model class for master data for employee roles
    /// </summary>
    [XmlRoot("value")]
    public class Role {
        [XmlElement("id")]
        public int id {get; set;}
        
        [XmlElement("name")]
        public String name {get; set;}
        
        [XmlElement("description")]
        public String description {get; set;}
    }
}