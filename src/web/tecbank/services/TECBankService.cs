using System.Collections.Generic;
using System;

using tecbank.services.DBMS;
using tecbank.models;
using Microsoft.AspNetCore.Mvc;

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

        private static List<BankMovement> movements = new List<BankMovement>{
            new BankMovement { id = "EF200", total_transfer = 7000 , date = DateTime.Parse("2025-02-21T10:36:00"), description = "Compra en servicios" , type = 1, card_id=40025, account_id ="152CR54126bt67", currency_id = 3}
        };

        private static List<BankLoan> loans = new List<BankLoan>{};
        private static List<LoanPayment> payments = new List<LoanPayment>{};

        private static List<BankEmployee> employees = new List<BankEmployee>{
            new BankEmployee { id=7, name="Juan", last_name1="Miranda", last_name2="Solis", role_id=1},
            new BankEmployee { id=9, name="Adolfo", last_name1="Vargas", last_name2="Paniagua", role_id=2},
            new BankEmployee { id=7, name="Daniel", last_name1="Cabrera", last_name2="Ortiz", role_id=2}
        };
        // --------------------------------[ Service atributes and properties]--------------------------------
        private static readonly String db_file = "tecbank";
        private static DBConnect tecbank_db = new DBConnect(db_file);
        // --------------------------------[ Service functions and methods ]--------------------------------

        // ::. CLIENT METHODS
        // public List<ClientAccount> GetAllClients() => clients;

        // Gets all clients listed in the XML
        public List<ClientAccount> GetAllClients() 
        {
            try
            {
                return tecbank_db.extract_all<ClientAccount>("clients");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return new List<ClientAccount>(); // Retorna lista vacía en caso de error
            }
        }

        public ClientAccount Client_findByID(int id) {
            try {
                var clients = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == id);
                return clients.FirstOrDefault(c => c.id == id);
            } catch (System.Exception e1){
                throw new SystemException($"TABLE.SELECT failed: {e1}");
            }   
        }

        public ClientAccount Client_find(String username, String password){
            var client = clients.FirstOrDefault(c => c.username == username && c.password == password);
            return client;
        }

        /*
        public void Client_Add(ClientAccount client) {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            clients.Add(client);
        }*/

        public void Client_Add(ClientAccount client) 
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            try
            {
                // 1. Validar que el cliente no exista ya (por ID u otro campo único)
                var existingClient = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == client.id).FirstOrDefault();
                if (existingClient != null)
                {
                    throw new InvalidOperationException($"Ya existe un cliente con el ID {client.id}");
                }

                // 2. Insertar el nuevo cliente en la base de datos XML
                tecbank_db.INSERT("clients", client);
            }
            catch (Exception ex)
            {
                // Loggear el error
                Console.WriteLine($"Error al agregar cliente: {ex.Message}");
                throw; // Re-lanzar la excepción para que el controlador la maneje
            }
        }

        public void Client_Update(ClientAccount client){
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var existingClient = Client_findByID(client.id);
            if (existingClient == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            // Actualizar propiedades
            existingClient.name = client.name;
            existingClient.last_name1 = client.last_name1;
            existingClient.last_name2 = client.last_name2;
            existingClient.type = client.type;
            existingClient.username = client.username;
            existingClient.password = client.password;
            existingClient.monthly_income = client.monthly_income;
            existingClient.phone_number = client.phone_number;
            existingClient.address = client.address;
        }

        public void Client_Delete(int id){
            var client = Client_findByID(id);
            if (client == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            clients.Remove(client);
        }

        // ::. BANK ACCOUNT METHODS
        //public List<BankAccount> GetAllAccounts() => accounts;

        public List<BankAccount> GetAllAccounts() 
        {
            try
            {
                return tecbank_db.extract_all<BankAccount>("accounts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las cuentas: {ex.Message}");
                return new List<BankAccount>(); // Retorna lista vacía en caso de error
            }
        }

        public List<BankAccount> AccountsFromClient(int user_id){
            var client_accounts = accounts.FindAll(acc => acc.client_id == user_id);
            return client_accounts;
        }

        public void Account_Add(BankAccount account){
            if (account == null)
                throw new ArgumentNullException(nameof(account));
            accounts.Add(account);
        }

        public void Account_Update(BankAccount account){
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var existingAccount = accounts.FirstOrDefault(a => a.id == account.id);
            if (existingAccount == null)
                throw new KeyNotFoundException("Cuenta no encontrada.");

            existingAccount.type = account.type;
            existingAccount.balance = account.balance;
            existingAccount.description = account.description;
            existingAccount.currency_id = account.currency_id;
            existingAccount.client_id = account.client_id;
        }

        public void Account_Delete(string id){
            var account = accounts.FirstOrDefault(a => a.id == id);
            if (account == null)
                throw new KeyNotFoundException("Cuenta no encontrada.");

            accounts.Remove(account);
        }

        // ::. BANK CARD METHODS
        //public List<BankCard> GetAllCards() => cards;

        public List<BankCard> GetAllCards() 
        {
            try
            {
                return tecbank_db.extract_all<BankCard>("cards");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las cuentas: {ex.Message}");
                return new List<BankCard>(); // Retorna lista vacía en caso de error
            }
        }

        public List<BankCard> CardsFromClient(int user_id){
            var client_accounts = accounts.FindAll(acc => acc.client_id == user_id);
            List<BankCard> client_cards = [];
            for (int i = 0; i < client_accounts.Count; i++){
                var acc = client_accounts[i];
                var acc_cards = cards.FindAll(cc => cc.account_id == acc.id);
                for (int j = 0; j < acc_cards.Count; j++){
                    client_cards.Add(acc_cards[j]);
                }
            }
            return client_cards;
        }

        public List<BankCard> CardsFromAccount(int user_id, String account_id){
            var account = accounts.FirstOrDefault(acc => acc.id == account_id && acc.client_id == user_id) ?? throw new NullReferenceException();
            var account_cards = cards.FindAll(cc => cc.account_id == account.id);
            return account_cards;
        }

        public void Card_Add(BankCard card){
            if (card == null) throw new ArgumentNullException(nameof(card));
            cards.Add(card);
        }

        
        public void Card_Delete(int cardNum){
            var card = cards.FirstOrDefault(c => c.card_num == cardNum);
            if (card == null) throw new KeyNotFoundException("Tarjeta no encontrada.");
            cards.Remove(card);
        }

        public void Card_Update(BankCard card){
            if (card == null) throw new ArgumentNullException(nameof(card));
            var existingCard = cards.FirstOrDefault(c => c.card_num == card.card_num);
            if (existingCard == null) throw new KeyNotFoundException("Tarjeta no encontrada.");
            
            existingCard.type = card.type;
            existingCard.cvc = card.cvc;
            existingCard.balance = card.balance;
            existingCard.account_id = card.account_id;
        }

        // ::. EMPLOYEE METHODS
        public List<BankEmployee> GetAllEmployes() => employees;

        // ::. LOAN PAYMENT METHODS
        public List<LoanPayment> GetAllPayments() => payments;

        public List<LoanPayment> Payments_FromClient(int user_id) {
            List<LoanPayment> client_payments = [];
            var client_loans = loans.FindAll(ln => ln.client_id == user_id);
            for (int i=0; i < client_loans.Count; i++){
                client_payments.InsertRange(0,payments.FindAll(p => p.loan_id == client_loans[i].id));
            }
            return client_payments;
        }

        public void Payment_MakeAPayment(int user_id, String account_id, LoanPayment payment){
            var target_loan = loans.FirstOrDefault(ln => ln.id == payment.loan_id);
            BankMovement related_movement = new BankMovement{ id = Guid.NewGuid().ToString(), description = "Pago de prestamo", date = payment.date, card_id = -1, total_transfer = payment.total, currency_id = target_loan.currency_id, account_id = account_id, type = 3};
            payment.movement_id = related_movement.id;
            target_loan.balance -= payment.total;
            if (target_loan.total <= 0){
                target_loan.state = 1;
            }

            payments.Add(payment);
            movements.Add(related_movement);
        }

        // ::. BANK LOAN METHODS
        public List<BankLoan> GetAllLoans() => loans;

        public List<BankLoan> Loans_FromClient(int user_id){
            var client_loans = loans.FindAll(ln => ln.client_id == user_id);
            return client_loans;
        }

        public void Loan_Add(BankLoan loan){
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));

            // Validar que el cliente y asesor existan
            if (!clients.Any(c => c.id == loan.client_id))
                throw new ArgumentException("Cliente no existe.");
            if (!employees.Any(e => e.id == loan.adviser_id))
                throw new ArgumentException("Asesor no existe.");

            loans.Add(loan);
        }

        public void Loan_Update(BankLoan loan){
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));

            var existingLoan = loans.FirstOrDefault(l => l.id == loan.id);
            if (existingLoan == null)
                throw new KeyNotFoundException("Préstamo no encontrado.");

            // Solo actualiza campos modificables (evita cambiar client_id o adviser_id)
            existingLoan.lapse = loan.lapse;
            existingLoan.interest_rate = loan.interest_rate;
            existingLoan.balance = loan.balance;
            existingLoan.total = loan.total;
            existingLoan.state = loan.state;
        }

        // ::. BANK MOVEMENT METHODS
        //public List<BankMovement> GetAllMovements() => movements;

        public List<BankMovement> GetAllMovements() 
        {
            try
            {
                return tecbank_db.extract_all<BankMovement>("movements");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las cuentas: {ex.Message}");
                return new List<BankMovement>(); // Retorna lista vacía en caso de error
            }
        }

        public void Movement_New(int user_id, BankMovement movement) {
            // >> COMPROBACION 1: Integridad del objeto <<
            if (movement == null) throw new ArgumentNullException(nameof(movement));
            var user_accounts = accounts.FindAll(acc => acc.client_id == user_id);

            // >> COMPROBACION 2: Existencia de cuentas <<
            if (user_accounts.Count == 0) throw new KeyNotFoundException("El usuario tramitante no tiene ninguna cuenta a su nombre");
            bool acc_exists = false;
            for (int i = 0; i < user_accounts.Count; i++){
                if (user_accounts[i].id == movement.account_id){
                    acc_exists = true;
                    break;
                }
            }
            // >> COMPROBACION 3: Movimiento.Cuenta corresponde Usuario.Cuenta <<
            if (!acc_exists) throw new KeyNotFoundException("La cuenta asignada al movimiento no corresponde a ninguna cuenta del cliente");
            movement.id = Guid.NewGuid().ToString();
            movements.Add(movement);
        }

        public List<BankMovement> Movements_FromAccount(String account_id, int client_id){
            var client = clients.FirstOrDefault(cli => cli.id == client_id);
            // >> COMPROBACION 1: Existencia del usuario <<
            if (client == null) throw new KeyNotFoundException($"El usuario({client_id}) no existe");
            var client_account = accounts.FirstOrDefault(acc => acc.id == account_id && acc.client_id == client_id);
            // >> COMPROBACION 2: Existencia de la cuenta <<
            if (client_account == null) throw new KeyNotFoundException($"El usuario({client_id}) no tiene la cuenta({account_id}) a su nombre");
            var account_movements = movements.FindAll(mov => mov.account_id == account_id);
            return account_movements;
        }
        
    }
}