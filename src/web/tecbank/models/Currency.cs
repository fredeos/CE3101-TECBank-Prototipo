
namespace tecbank.models{
    /// <summary> 
    /// Model class for a bank currency in the database
    /// </summary>
    /// id, name, usd_exchange
    public class Currency {
        public int id {get; set;}
        public String name {get; set;} = string.Empty; // Warning solution
        public float usd_exchange {get; set;}
    }
}