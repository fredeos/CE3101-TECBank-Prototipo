using System.Collections.Generic;
using tecbank.models;
using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for a employee in the database
    /// </summary>
    [XmlRoot("value")]
    public class BankEmployee {
        [XmlElement("id")]
        public int id {get; set;}

        [XmlElement("name")]
        public String name {get; set;} = string.Empty; // Warning solution

        [XmlElement("last_name1")]
        public String last_name1 {get; set;} = string.Empty; // Warning solution

        [XmlElement("last_name2")]
        public String last_name2 {get; set;} = string.Empty; // Warning solution

        [XmlElement("role_id")]
        public int role_id {get; set;}
    }
    /// <summary>
    /// Class for subtype of employee that works as a loan adviser
    /// </summary>
    public class LoanAdviser : BankEmployee {
        public List<BankLoan> loans = [];
        public List<AdviserGoal> goals = [];
    }

}