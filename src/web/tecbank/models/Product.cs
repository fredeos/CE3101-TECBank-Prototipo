namespace tecbank.models {
    /// <summary> 
    /// Model class for a product in the database
    /// </summary>
    /// id, type, balance, description, currency_id, client_id
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Warning solution
        public decimal Price { get; set; }
    }
}