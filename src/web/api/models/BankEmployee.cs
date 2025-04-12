using System.Collections.Generic;
using System.Xml.Serialization;
using tecbank.models;

namespace tecbank.models{
    /// <summary>
    /// Model class for a employee in the database
    /// </summary>
    [XmlRoot("value")]
    public class BankEmployee {
        [XmlElement("id")]
        public int id {get; set;}

        [XmlElement("name")]
        public String name {get; set;}

        [XmlElement("last_name1")]
        public String last_name1 {get; set;}

        [XmlElement("last_name2")]
        public String last_name2 {get; set;}

        [XmlElement("role_id")]
        public int role_id {get; set;}

        [XmlElement("rem_state")]
        public int removed {get; set;}
    }

}