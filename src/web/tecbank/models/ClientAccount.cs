
using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for a client account in the database
    /// </summary>
    [XmlRoot("value")]
    public class ClientAccount{
        [XmlElement("id")]
        public int id {get; set;}

        [XmlElement("name")]
        public String name {get; set;} = string.Empty; // Warning solution

        [XmlElement("last_name1")]
        public String last_name1 {get; set;} = string.Empty; // Warning solution

        [XmlElement("last_name2")]
        public String last_name2 {get; set;} = string.Empty; // Warning solution

        [XmlElement("type")]
        public int type {get; set;}

        [XmlElement("username")]
        public String username {get; set;} = string.Empty; // Warning solution

        [XmlElement("password")]
        public String password {get; set;} = string.Empty; // Warning solution

        [XmlElement("monthly_income")]
        public float monthly_income{get; set;}

        [XmlElement("phone_number")]
        public String phone_number{get; set;} = string.Empty; // Warning solution

        [XmlElement("address")]
        public String address {get; set;} = string.Empty; // Warning solution
    }
}