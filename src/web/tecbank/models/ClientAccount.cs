
namespace tecbank.models{
    /// <summary>
    /// Model class for a client account in the database
    /// </summary>
    public class ClientAccount{
        public int id {get; set;}
        public String name {get; set;}
        public String last_name1 {get; set;}
        public String last_name2 {get; set;}
        public int type {get; set;}
        public String username {get; set;}
        public String password {get; set;}
        public int monthly_income{get; set;}
        public String phone_number{get; set;}
        public String address {get; set;}
    }
}