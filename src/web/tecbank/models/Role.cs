
namespace tecbank.models {
    /// <summary> 
    /// Model class for a employee role in the database
    /// </summary>
    /// id, name, description
    public class Role {
        public int id {get; set;}
        public String name {get; set;} = string.Empty; // Warning solution
        public String description {get; set;} = string.Empty; // Warning solution
    }
}