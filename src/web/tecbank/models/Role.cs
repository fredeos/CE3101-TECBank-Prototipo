using System.Xml.Serialization;

namespace tecbank.models {
    /// <summary> 
    /// Model class for a employee role in the database
    /// </summary>
    /// id, name, description
    [XmlRoot("value")]
    public class Role {
        [XmlElement("id")]
        public int id {get; set;}

        [XmlElement("name")]
        public String name {get; set;} = string.Empty; // Warning solution
        
        [XmlElement("description")]
        public String description {get; set;} = string.Empty; // Warning solution
    }
}