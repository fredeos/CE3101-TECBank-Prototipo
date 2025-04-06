using System.Collections.Generic;

using tecbank.services.DBMS;
using tecbank.models;

namespace tecbank.services{
    /// <summary>
    /// This class works as service access point for the tecbank database
    /// It serves common resource for all backend api endpoints and controllers
    /// </summary>
    public class TECBankService {
        // --------------------------------[Temporary simulated entities (TODO: replace with actual database access point)]
        private static List<ClientAccount> clients = new List<ClientAccount>
        {
            new ClientAccount {id = 1, name = "Isaac", last_name1 = "Ramirez", last_name2="Herrera", type = 1, username = "snk100", password = "1234", monthly_income = 10000000, phone_number = "+506 3163 7168", address = "Cartago"},
            new ClientAccount {id = 2, name = "Marco", last_name1 = "Rivera", last_name2="Meneses", type = 1, username = "marc300", password = "1234", monthly_income = 6300000, phone_number = "+506 8163 7268", address = "San Jose"}
        }; 

        private static List<BankAccount> accounts = new List<BankAccount>
        {
            new BankAccount { id = "152CR54126bt67", type = 1, balance = 80000, description = "Personal", currency_id = 1, client_id = 1},
            new BankAccount { id = "646CR54186bt11", type = 1, balance = 65000, description = "Trabajo", currency_id = 3, client_id = 2}
        };

        private static List<BankCard> cards = new List<BankCard>
        {
            new BankCard { card_num = 40030, type = 1, cvc = 376, balance = -60000, account_id = "152CR54126bt67"},
            new BankCard { card_num = 40025, type = 2, cvc = 680, balance = 40000, account_id = "152CR54126bt67"},
            new BankCard { card_num = 10307, type = 2, cvc = 501, balance = 60000, account_id = "646CR54186bt11"}
        };

        private static List<BankMovement> movements = new List<BankMovement>
        {
            new BankMovement { id = "EF200", total_transfer = 7000 , date = DateTime.Parse("2025-02-21T10:36:00"), description = "Compra en servicios" , type = 1, card_id=40025, account_id ="152CR54126bt67", currency_id = 3}
        };

        private static List<BankLoan> loans = new List<BankLoan>{};
        private static List<LoanPayment> payments = new List<LoanPayment>{};

        private static List<Employee> employees = new List<Employee>{
            new Employee { id=7, name="Juan", last_name1="Miranda", last_name2="Solis", role_id=1},
            new Employee { id=9, name="Adolfo", last_name1="Vargas", last_name2="Paniagua", role_id=2},
            new Employee { id=7, name="Daniel", last_name1="Cabrera", last_name2="Ortiz", role_id=2}
        };
        // --------------------------------[ Service atributes and properties]--------------------------------
        private static readonly String db_file = "tecbank";
        private static readonly DBConnect tecbank_db = new DBConnect(db_file);
        // --------------------------------[ Service functions and methods ]--------------------------------
        public List<ClientAccount> GetAllClients() => clients;
        public List<BankAccount> GetAllAccounts() => accounts;
        public List<BankCard> GetAllCards() => cards;
        public List<Employee> GetAllEmployes() => employees;
        public List<LoanPayment> GetAllPayments() => payments;
        public List<BankLoan> GetAllLoans() => loans;
        public List<BankMovement> GetAllMovements() => movements;

        public ClientAccount Client_findByID(int id){
            var client = clients.FirstOrDefault(c => c.id == id);
            return client;
        }

        public ClientAccount Client_find(String username, String password){
            var client = clients.FirstOrDefault(c => c.username == username && c.password == password);
            return client;
        }
    }
}