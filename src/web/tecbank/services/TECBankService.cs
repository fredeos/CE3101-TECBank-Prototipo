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

        /// <summary>
        /// Retrieves all client accounts from the database.
        /// </summary>
        /// <returns>
        /// A list of ClientAccount objects containing all client records.
        /// Returns an empty list if an error occurs during retrieval.
        /// </returns>
        /// <remarks>
        /// This method attempts to extract all client records from the "clients" table.
        /// If any error occurs during the process, it logs the error to console
        /// and returns an empty list to prevent null reference exceptions in calling code.
        /// </remarks>
        public List<ClientAccount> GetAllClients() 
        {
            try
            {
                // Attempt to extract all client records from the "clients" table
                return tecbank_db.extract_all<ClientAccount>("clients");
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the extraction process
                Console.WriteLine($"Error retrieving clients: {ex.Message}");
                // Return an empty list as a safe default value
                return new List<ClientAccount>();
            }
        }

        /// <summary>
        /// Finds a client account by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the client to search for</param>
        /// <returns>The found ClientAccount object</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no client with the specified ID exists</exception>
        /// <exception cref="SystemException">Thrown when there's an error accessing the database</exception>
        /// <remarks>
        /// This method:
        /// 1. Queries the "clients" table for records matching the provided ID
        /// 2. Returns the first matching client if found
        /// 3. Throws a KeyNotFoundException if no client exists with that ID
        /// 4. Wraps any database errors in a SystemException with context
        /// </remarks>
        public ClientAccount Client_findByID(int id)
        {
            try 
            {
                // Query the database for clients matching the specified ID
                var clients = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == id);
                // Return the first match or throw exception if not found
                return clients.FirstOrDefault() ?? 
                    throw new KeyNotFoundException($"Client with ID {id} not found");
            }
            catch (Exception ex)
            {
                // Wrap any database errors in a SystemException with context
                throw new SystemException($"Client lookup failed:: {ex.Message}");
            }   
        }

        // Falta
        public ClientAccount Client_find(string username, string password)
        {
            return clients.FirstOrDefault(c => c.username == username && c.password == password) 
                ?? throw new InvalidOperationException("Credenciales inválidas");
        }

        /// <summary>
        /// Adds a new client account to the system
        /// </summary>
        /// <param name="client">The client account to add</param>
        /// <exception cref="ArgumentNullException">Thrown when client is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when client ID already exists</exception>
        /// <exception cref="Exception">Thrown for any database operation failures</exception>
        /// <remarks>
        /// Performs two main operations:
        /// 1. Validates the client doesn't already exist
        /// 2. Inserts the new client record into the XML database
        /// </remarks>
        public void Client_Add(ClientAccount client) 
        {
            // Validate input parameter
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            try
            {
                // 1. Check for existing client with same ID
                var existingClient = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == client.id).FirstOrDefault();
                if (existingClient != null)
                {
                    throw new InvalidOperationException($"Ya existe un cliente con el ID {client.id}");
                }

                // 2. Insert new client into XML database
                tecbank_db.INSERT("clients", client);
            }
            catch (Exception ex)
            {
               // Log error details to console (consider using ILogger in production)
                Console.WriteLine($"Error al agregar cliente: {ex.Message}");
                throw; // Re-throw to allow controller to handle
            }
        }

        // Falta
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
        
        //Falta
        public void Client_Delete(int id){
            var client = Client_findByID(id);
            if (client == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            clients.Remove(client);
        }

        // ::. BANK ACCOUNT METHODS

        /// <summary>
        /// Retrieves all bank accounts from the database
        /// </summary>
        /// <returns>List of all accounts or empty list if error occurs</returns>
        public List<BankAccount> GetAllAccounts() 
        {
            try
            {
                return tecbank_db.extract_all<BankAccount>("accounts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving accounts: {ex.Message}");
                return new List<BankAccount>(); // Retorna lista vacía en caso de error
            }
        }

        /// <summary>
        /// Retrieves a bank account by its unique ID
        /// </summary>
        /// <param name="id">Account ID to search for</param>
        /// <returns>Matching BankAccount object</returns>
        /// <exception cref="KeyNotFoundException">Account not found</exception>
        /// <exception cref="SystemException">Database operation failed</exception>
        public BankAccount GetAccountById(string id)
        {
            try
            {
                var account = tecbank_db.SELECT<BankAccount>("accounts", a => a.id == id).FirstOrDefault();
                return account ?? throw new KeyNotFoundException($"Account with ID {id} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving account {id}: {ex.Message}");
                throw new SystemException("Failed to retrieve account", ex);
            }
        }

        // Falta
        public List<BankAccount> AccountsFromClient(int user_id){
            var client_accounts = accounts.FindAll(acc => acc.client_id == user_id);
            return client_accounts;
        }

        /// <summary>
        /// Adds a new bank account with validation checks
        /// </summary>
        /// <param name="account">Account to add</param>
        /// <exception cref="ArgumentNullException">Null account provided</exception>
        /// <exception cref="ArgumentException">Validation failure</exception>
        /// <exception cref="SystemException">Database operation failed</exception>
        public void Account_Add(BankAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            try
            {
                // 1. Set default description if empty
                if (string.IsNullOrWhiteSpace(account.description))
                    account.description = "Personal account"; // Valor por defecto

                // 2. Validate client exists
                var clientExists = tecbank_db.SELECT<ClientAccount>("clients", 
                                c => c.id == account.client_id).Any();
                if (!clientExists)
                    throw new ArgumentException($"Client with ID {account.client_id} not found");

                // 3. Validate currency exists
                var currencyExists = tecbank_db.SELECT<Currency>("currency", 
                                    c => c.id == account.currency_id).Any();
                if (!currencyExists)
                    throw new ArgumentException($"Currency with ID {account.currency_id} not found");

                // 4. Validate account ID is unique
                var accountExists = tecbank_db.SELECT<BankAccount>("accounts", 
                                a => a.id == account.id).Any();
                if (accountExists)
                    throw new ArgumentException($"Account {account.id} already exists");

                // 5. Set default balance if negative
                if (account.balance < 0)
                    account.balance = 0;

                // 6. Insert into database
                tecbank_db.INSERT("accounts", account);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error adding account: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Generates a Costa Rican-style bank account ID
        /// Format: 3-digit bank code + 9-digit account number + 1 check digit
        /// </summary>
        /// <returns>Generated account ID string</returns>
        public string GenerateCostaRicanAccountId()
        {
            // Standard CR account format:
            // 3-digit bank code + 9-digit account + 1 check digit
            
            const string bankCode = "151"; // Example bank code
            var random = new Random();
            
            // Generate 9-digit account number
            var accountNumber = random.Next(0, 999999999).ToString("D9"); 
            
            // Simple check digit
            var checkDigit = random.Next(0, 9); 

            return $"{bankCode}{accountNumber}{checkDigit}";
        }

        
        // Falta
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

        // Falta
        public void Account_Delete(string id){
            var account = accounts.FirstOrDefault(a => a.id == id);
            if (account == null)
                throw new KeyNotFoundException("Cuenta no encontrada.");

            accounts.Remove(account);
        }

        // ::. BANK CARD METHODS

        /// <summary>
        /// Retrieves all bank cards from the database
        /// </summary>
        /// <returns>List of bank cards or empty list on error</returns>
        public List<BankCard> GetAllCards() 
        {
            try
            {
                return tecbank_db.extract_all<BankCard>("cards");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cards: {ex.Message}");
                return new List<BankCard>(); // Returns empty list if error occurs
            }
        }

        /// <summary>
        /// Retrieves a bank card by its card number
        /// </summary>
        /// <param name="cardNumber">The card number to search for</param>
        /// <returns>The matching BankCard object</returns>
        /// <exception cref="KeyNotFoundException">Card not found</exception>
        /// <exception cref="SystemException">Database operation failed</exception>
        public BankCard GetCardByNumber(int cardNumber)
        {
            try
            {
                var card = tecbank_db.SELECT<BankCard>("cards", c => c.card_num == cardNumber).FirstOrDefault();
                return card ?? throw new KeyNotFoundException($"Card with number {cardNumber} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving card {cardNumber}: {ex.Message}");
                throw new SystemException("Failed to retrieve card", ex);
            }
        }

        // Falta
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

        // Falta
        public List<BankCard> CardsFromAccount(int user_id, String account_id){
            var account = accounts.FirstOrDefault(acc => acc.id == account_id && acc.client_id == user_id) ?? throw new NullReferenceException();
            var account_cards = cards.FindAll(cc => cc.account_id == account.id);
            return account_cards;
        }


        /// <summary>
        /// Adds a new bank card with validation and auto-generation of missing fields
        /// </summary>
        /// <param name="card">Card to add</param>
        /// <exception cref="ArgumentNullException">Null card provided</exception>
        /// <exception cref="ArgumentException">Validation failure</exception>
        /// <exception cref="SystemException">Database operation failed</exception>
        public void Card_Add(BankCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            try
            {
                // Validate linked account exists
                if (!tecbank_db.SELECT<BankAccount>("accounts", a => a.id == card.account_id).Any())
                    throw new ArgumentException($"Account {card.account_id} not found");

                // Auto-generate card number if not provided
                if (card.card_num == 0)
                {
                    card.card_num = GenerateCardNumber();
                }
                else
                {
                    // Validate card number format (8-9 digits)
                    if (card.card_num < 10000000 || card.card_num > 999999999)
                        throw new ArgumentException("Card number must be 8-9 digits");
                }

                // Auto-generate CVC if not provided
                if (card.cvc == 0)
                    card.cvc = GenerateCVC();
                else if (card.cvc < 100 || card.cvc > 999)
                    throw new ArgumentException("CVC must be 3 digits");

                // Validate card type (1: Debit, 2: Credit)
                if (card.type < 1 || card.type > 2)
                    throw new ArgumentException("Invalid card type (1: Debit, 2: Credit)");

                // Ensure non-negative balance
                if (card.balance < 0)
                    card.balance = 0;

                tecbank_db.INSERT("cards", card);
            }
            catch (Exception ex)
            {
                throw new SystemException($"Error adding card: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generates a valid card number (8 digits starting with 4)
        /// </summary>
        /// <returns>Generated card number with check digit</returns>
        public int GenerateCardNumber()
        {
            Random random = new Random();
            
            // Generate 8-digit number (safe Int32 range)
            int cardNumber = random.Next(40000000, 49999999); // Starts with 4 (Visa-style)
            
            // Append simple check digit
            return cardNumber * 10 + CalculateSimpleCheckDigit(cardNumber);
        }

        /// <summary>
        /// Calculates a simple check digit for card numbers
        /// </summary>
        /// <param name="number">Card number base</param>
        /// <returns>Single check digit (0-9)</returns>
        public int CalculateSimpleCheckDigit(int number)
        {
            int sum = 0;
            int temp = number;
            
            // Sum all digits
            while (temp > 0)
            {
                sum += temp % 10;
                temp /= 10;
            }
            
            return sum % 10; // Modulo 10 check digit
        }

        /// <summary>
        /// Generates a random 3-digit CVC code
        /// </summary>
        /// <returns>CVC between 100-999</returns>
        public int GenerateCVC()
        {
            return new Random().Next(100, 999);
        }

        // Falta
        public void Card_Delete(int cardNum){
            var card = cards.FirstOrDefault(c => c.card_num == cardNum);
            if (card == null) throw new KeyNotFoundException("Tarjeta no encontrada.");
            cards.Remove(card);
        }

        // Falta
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

        /// <summary>
        /// Retrieves all bank employees from the database
        /// </summary>
        /// <returns>List of employees or empty list on error</returns>
        public List<BankEmployee> GetAllEmployes() 
        {
            try
            {
                return tecbank_db.extract_all<BankEmployee>("employees");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving employees: {ex.Message}");
                return new List<BankEmployee>(); // Retorna lista vacía en caso de error
            }
        }

        // ::. LOAN PAYMENT METHODS

        /// <summary>
        /// Retrieves all loan payments from the database
        /// </summary>
        /// <returns>List of payments or empty list on error</returns>
        public List<LoanPayment> GetAllPayments() 
        {
            try
            {
                return tecbank_db.extract_all<LoanPayment>("payments");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payments: {ex.Message}");
                return new List<LoanPayment>(); // Returns empty list if error occurs
            }
        }

        // Falta
        public List<LoanPayment> Payments_FromClient(int user_id) {
            List<LoanPayment> client_payments = [];
            var client_loans = loans.FindAll(ln => ln.client_id == user_id);
            for (int i=0; i < client_loans.Count; i++){
                client_payments.InsertRange(0,payments.FindAll(p => p.loan_id == client_loans[i].id));
            }
            return client_payments;
        }

        // Falta
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

        /// <summary>
        /// Retrieves all loan payments from the database
        /// </summary>
        /// <returns>List of payments or empty list on error</returns>
        public List<BankLoan> GetAllLoans() 
        {
            try
            {
                return tecbank_db.extract_all<BankLoan>("loans");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payments: {ex.Message}");
                return new List<BankLoan>(); // Returns empty list if error occurs
            }
        }

        // Falta
        public List<BankLoan> Loans_FromClient(int user_id){
            var client_loans = loans.FindAll(ln => ln.client_id == user_id);
            return client_loans;
        }

        /// <summary>
        /// Retrieves a loan by its unique ID
        /// </summary>
        /// <param name="id">The ID of the loan to retrieve</param>
        /// <returns>The found BankLoan object</returns>
        /// <exception cref="KeyNotFoundException">Thrown when loan is not found</exception>
        /// <exception cref="SystemException">Thrown when database operation fails</exception>
        public BankLoan GetLoanById(int id)
        {
            try
            {
                // Execute SELECT query directly with ID criteria
                // FirstOrDefault returns null if no match is found

                var loan = tecbank_db.SELECT<BankLoan>("loans", l => l.id == id).FirstOrDefault();

                // Return the loan if found, otherwise throw KeyNotFoundException
                return loan ?? throw new KeyNotFoundException($"Loan with ID {id} not found");
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw specific exceptions without modification to preserve stack trace
            }
            catch (Exception ex)
            {   
                // Wrap general exceptions in a SystemException with context information
                throw new SystemException($"Failed to retrieve loan {id}", ex);
            }
        }

        /// <summary>
        /// Adds a new loan to the system with validation
        /// </summary>
        /// <param name="loan">The loan to add</param>
        /// <exception cref="ArgumentNullException">Thrown when loan is null</exception>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        /// <exception cref="SystemException">Thrown when database operation fails</exception>
        public void Loan_Add(BankLoan loan)
        {
            // Validate input parameter
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));

            try
            {
                // 1. Validate that the client exists
                var clientExists = tecbank_db.SELECT<ClientAccount>("clients", 
                                c => c.id == loan.client_id).Any();
                
                if (!clientExists)
                    throw new ArgumentException($"Client with ID {loan.client_id} doesn't exist");

                // 2. Validate that the adviser exists and has the LoanAdviser role (role_id = 2)
                var adviser = tecbank_db.SELECT<BankEmployee>("employees", 
                            e => e.id == loan.adviser_id && e.role_id == 2).FirstOrDefault();
                
                if (adviser == null)
                    throw new ArgumentException($"Loan adviser with ID {loan.adviser_id} not found or not authorized");

                // 3. Validate loan data
                if (loan.total <= 0)
                    throw new ArgumentException("Loan amount must be positive");

                // 4. Set default values
                loan.request_date = DateTime.Now;
                loan.balance = loan.total;

                // 5. Insert into database
                tecbank_db.INSERT("loans", loan);
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions to the caller
            }
            catch (Exception ex)
            {
                // Wrap other exceptions with context information
                throw new SystemException("Failed to add loan", ex);
            }
        }

        // Falta
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

        /// <summary>
        /// Retrieves all bank movements from the database.
        /// </summary>
        /// <returns>
        /// A list of BankMovement objects if successful; 
        /// an empty list if an error occurs.
        /// </returns>
        /// <remarks>
        /// This method handles exceptions internally by returning an empty list
        /// and logging the error to the console. Consider using a proper logging
        /// framework in production code.
        /// </remarks>
        public List<BankMovement> GetAllMovements() 
        {
            try
            {
                // Extract all movement records from the database
                // using the generic extract_all method
                return tecbank_db.extract_all<BankMovement>("movements");
            }
            catch (Exception ex)
            {
                // Log error details to console (for debugging)
                Console.WriteLine($"Error al obtener las cuentas: {ex.Message}");

                // Return empty list as fallback value
                // This ensures calling code always gets a valid List object
                return new List<BankMovement>(); // Retorna lista vacía en caso de error
            }
        }

        // Falta
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

        // Falta
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