using System.Xml.Serialization;

namespace tecbank.models{
    /// <summary>
    /// Model class for goals set for an adviser
    /// </summary>
    /// adviser_id, target_profit, start_date, limit_date, state, currency_id
    [XmlRoot("value")]
    public class AdviserGoal {
        [XmlElement("adviser_id")]
        public int adviser_id {get; set;}

        [XmlElement("target_profit")]
        public float target_profit {get; set;}

        [XmlElement("start_date")]
        public DateTime start_date {get; set;}

        [XmlElement("limit_date")]
        public DateTime limit_date {get; set;}

        [XmlElement("state")]
        public int state {get; set;}

        [XmlElement("currency_id")]
        public int currency_id {get; set;}
    }
}