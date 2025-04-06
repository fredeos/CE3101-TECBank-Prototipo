using System.Collections.Generic;
using tecbank.models;

namespace tecbank.models{
    /// <summary>
    /// Model class for a employee in the database
    /// </summary>
    public class Employee {
        public int id {get; set;}
        public String name {get; set;}
        public String last_name1 {get; set;}
        public String last_name2 {get; set;}
        public int role_id {get; set;}
    }
    /// <summary>
    /// Class for subtype of employee that works as a loan adviser
    /// </summary>
    public class LoanAdviser : Employee {
        public List<BankLoan> loans = [];
        public List<AdviserGoal> goals = [];
    }

}