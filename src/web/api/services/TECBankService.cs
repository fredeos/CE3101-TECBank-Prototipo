using System.Collections.Generic;
using System;

using tecbank.services.DBMS;
using tecbank.models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace tecbank.services{
    /// <summary>
    /// Exception class for webapi services
    /// </summary>
    public class ServiceException : Exception{
        public ServiceException() { }

        public ServiceException(string message) 
            : base(message) { }

        public ServiceException(string message, Exception inner) 
            : base(message, inner) { }
    }

    /// <summary>
    /// This class works as service access point for the tecbank database
    /// 
    /// It serves common resource for all backend api endpoints and controllers
    /// </summary>
    public class TECBankService {
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
        public List<ClientAccount> GetAllClients(){
            try {
                var clients = tecbank_db.SELECT<ClientAccount>("clients");
                return clients;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Finds a client account by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the client to search for</param>
        /// <returns>The found ClientAccount object</returns>
        /// <exception cref="DBMSException">Thrown when there's an error accessing the database</exception>
        /// <remarks>
        /// This method:
        /// 1. Queries the "clients" table for records matching the provided ID
        /// 2. Returns the first matching client if found
        /// 3. Wraps any database errors in a DBMSException with context
        /// </remarks>
        public ClientAccount? Client_findByID(int id) {
            try {
                var client = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == id && c.removed == 0).FirstOrDefault();
                return client;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Finds a client based on its username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        /// <remarks> Attempts login and retrives client data </remarks>
        public ClientAccount? Client_find(String username, String password){
            try {
                var client = tecbank_db.SELECT<ClientAccount>("clients", c => c.username == username && c.password == password && c.removed == 0).FirstOrDefault();
                return client;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
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
        public void Client_Add(ClientAccount client) {
            if (client == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(client)} is null and therefore not a valid object in the database");
            try{
                tecbank_db.INSERT<ClientAccount>("clients", client);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
            
        }

        /// <summary>
        /// Updates and modifies the information of a client
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <remarks> 
        /// All attributes of client must have a value and only different attributes will be modified
        /// </remarks>
        public void Client_Update(ClientAccount client){
            try{
                if (client == null)
                    throw new ArgumentNullException($"(TECBANKSERVICE) Client({nameof(client)}) object is null");
                
                var existingClient = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == client.id);
                if (existingClient == null)
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Client(ID={client.id}) not found");

                tecbank_db.MODIFY<ClientAccount>("clients", client, (a,b) => a.id == b.id);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Deletes a client from the database
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <remarks> 
        /// The client must noy have any pending loans. All related cards and accounts will removed as well.
        /// </remarks>
        public void Client_Delete(int id){
            try{
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == id).FirstOrDefault() ??
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Client(ID={id}) not found in the database");
                if (tecbank_db.SELECT<BankLoan>("loans",bl => bl.client_id == id && bl.balance > 0).Count >= 1){
                    throw new InvalidOperationException($"(TECBANKSERVICE) Client(ID={id}) cannot be removed until all loans are paid");
                }
                tecbank_db.REMOVE<ClientAccount>("clients", c => c.id == id);
                foreach(var acc in tecbank_db.SELECT<BankAccount>("accounts",ba => ba.client_id == id)){
                    tecbank_db.REMOVE<BankCard>("cards",bc => bc.account_id == acc.id);
                }
                tecbank_db.REMOVE<BankAccount>("accounts",ba => ba.client_id == id);
            } catch (DBMSException e1 ){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2 ){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        // ::. BANK ACCOUNT METHODS

        /// <summary>
        /// Retrieves all bank accounts from the database
        /// </summary>
        /// <exception cref="ServiceException"></exception>
        /// <returns>List of all accounts or empty list if error occurs</returns>
        public List<BankAccount> GetAllAccounts() {
            try {
                var accounts = tecbank_db.SELECT<BankAccount>("accounts");
                return accounts;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Retrieves a bank account by its unique ID
        /// </summary>
        /// <param name="id">Account ID to search for</param>
        /// <returns>Matching BankAccount object</returns>
        /// <exception cref="SystemException">Database operation failed</exception>
        public BankAccount? Account_Get(string id){
            try {
                var acc = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.id == id && ba.removed == 0).FirstOrDefault();
                return acc;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Retrives all accounts from a client
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<BankAccount> Accounts_FromClient(int user_id){
            try{
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                var accounts = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.client_id == client.id && ba.removed == 0);
                return accounts;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }


        /// <summary>
        /// Generates a Costa Rican-style bank account ID
        /// Format: 3-digit bank code + 9-digit account number + 1 check digit
        /// </summary>
        /// <returns>Generated account ID string</returns>
        public string GenerateBankAccountId(String nationality){
            // Standard CR account format:
            // 3-digit bank code + 9-digit account + 1 check digit
            const string bankCode = "151"; // Example bank code
            var random = new Random();
            
            // Generate 9-digit account number
            var accountNumber = random.Next(0, 999999999).ToString("D9"); 
            
            // Simple check digit
            var checkDigit = random.Next(0, 9); 

            return $"{bankCode}{nationality}{accountNumber}{checkDigit}";
        }

        /// <summary>
        /// Adds a new bank account for a client in the database
        /// </summary>
        /// <param name="account"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Account_Add(BankAccount account){
            if (account == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(account)} is null and therefore not a valid object in the database");
            try{
                account.id = GenerateBankAccountId("CR");
                // >> Set default description if empty
                if (string.IsNullOrWhiteSpace(account.description))
                    account.description = "Personal account"; // Valor por defecto
                // >> Validate currency exists
                var currency = tecbank_db.SELECT<Currency>("currency", c => c.id == account.currency_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={account.currency_id}) doesn't exist on the database");
                // >> Set default balance if negative
                if (account.balance < 0)
                    account.balance = 0;
                tecbank_db.INSERT<BankAccount>("accounts", account);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        /// <summary>
        /// Update and modify the contents of a bank account
        /// </summary>
        /// <param name="account"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <remarks> 
        /// All attributes of account must have a value and only different attributes will be modified
        /// </remarks>
        public void Account_Update(BankAccount account){
            // >> Verificar integridad del objeto
            if (account == null)
                throw new ArgumentNullException($"(TECBANKSERVICE) Bank account({nameof(account)}) object is null");
            try{
                // >> Verificar existencia de la cuenta
                var existingAccount = tecbank_db.SELECT<BankAccount>("accounts", ba  => ba.id == account.id);
                if (existingAccount == null)
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Bank account(ID={account.id}) not found");

                tecbank_db.MODIFY<BankAccount>("accounts", account, (a,b) => a.id == b.id);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Delete a bank account and all its related cards
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <remarks>
        /// If the bank account has pending credit card debts it will stop the process
        /// </remarks>
        public void Account_Delete(String id){
            try{
                // >> Buscar la cuenta bancaria indicada
                var acc = tecbank_db.SELECT<BankAccount>("clients", ba => ba.id == id).FirstOrDefault() ??
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Bank account(ID={id}) not found in the database");
                // >> Verificar que ninguna tarjeta de credito tenga saldo negativos (deudas)
                var credit_cards = tecbank_db.SELECT<BankCard>("cards",bc => bc.account_id == id && bc.type == 1 && bc.balance < 0);
                if (credit_cards.Count > 0)
                    throw new InvalidOperationException($"(TECBANKSERVICE) Bank account(ID={id}) cant be removed since it has pending credit cards(#{credit_cards.Count}) debts");
                // >> Remover los elementso relacionados
                tecbank_db.REMOVE<BankAccount>("accounts", ba => ba.id == id);
                tecbank_db.REMOVE<BankCard>("cards", cc => cc.account_id == acc.id);
            } catch (DBMSException e1 ){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2 ){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        // ::. BANK CARD METHODS

        /// <summary>
        /// Retrieves all bank cards from the database
        /// </summary>
        /// <returns>List of bank cards or empty list on error</returns>
        public List<BankCard> GetAllCards(){
            try {
                var cards = tecbank_db.SELECT<BankCard>("cards");
                return cards;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Retrieves a bank card by its card number
        /// </summary>
        /// <param name="cardNumber">The card number to search for</param>
        /// <returns>The matching BankCard object</returns>
        /// <exception cref="SystemException">Database operation failed</exception>
        public BankCard? Card_Get(int card_num){
            try {
                var card = tecbank_db.SELECT<BankCard>("cards", bc => bc.card_num == card_num && bc.removed == 0).FirstOrDefault();
                return card;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Get all the bank cards from client
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<BankCard> Cards_FromClient(int user_id){
            try{
                // >> Buscar cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                // >> Buscar todas las cuentas del cliente
                var accounts = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.client_id == client.id);
                // >> Agregar todas las tarjetas de cada cuenta
                List<BankCard> cards = [];
                foreach (var account in accounts){
                    cards = cards.Concat(tecbank_db.SELECT<BankCard>("cards",bc => bc.account_id == account.id)).ToList();
                }
                return cards;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Get all the bank cards related to a credit account
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="account_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<BankCard> Cards_FromAccount(int user_id, String account_id){
            try{
                // >> Buscar cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                // >> Buscar la cuenta bancaria del cliente
                var account = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.client_id == client.id && ba.id == account_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={account_id}) not found in database");
                // >> Agregar todas las tarjetas de la cuenta
                var cards = tecbank_db.SELECT<BankCard>("cards",bc => bc.account_id == account.id);
                return cards;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Generates a valid card number (8 digits starting with 4)
        /// </summary>
        /// <returns>Generated card number with check digit</returns>
        public int GenerateCardNumber(){
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
        public int CalculateSimpleCheckDigit(int number){
            int sum = 0;
            int temp = number;
            // Sum all digits
            while (temp > 0){
                sum += temp % 10;
                temp /= 10;
            }
            
            return sum % 10; // Modulo 10 check digit
        }

        /// <summary>
        /// Generates a random 3-digit CVC code
        /// </summary>
        /// <returns>CVC between 100-999</returns>
        public int GenerateCVC(){
            return new Random().Next(100, 999);
        }

        /// <summary>
        /// Creates a new card for a bank account
        /// </summary>
        /// <param name="card"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Card_Add(BankCard card){
            if (card == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(card)} is null and therefore not a valid object in the database");
            try{
                // >> Generar datos de la tarejeta
                card.card_num = GenerateCardNumber();
                card.cvc = GenerateCVC();
                // >> Ensure non-negative balance
                if (card.balance < 0)
                    card.balance = 0;
                tecbank_db.INSERT<BankCard>("cards", card);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        /// <summary>
        /// Delete a card from the database
        /// </summary>
        /// <param name="card_num"></param>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <remarks>
        /// Credit cards with credit debt wont be removed
        /// </remarks>
        public void Card_Delete(int card_num){
            try{
                // >> Verificar si la tarjeta es de credito y si tiene saldo negativo
                var card = tecbank_db.SELECT<BankCard>("cards",bc => bc.card_num == card_num && bc.type == 1).FirstOrDefault();
                if (card != null && card.balance < 0)
                    throw new InvalidOperationException($"(TECBANKSERVICE) Credit card(ID={card_num}) has pending credit debt");
                // >> Remover la tarjeta de credito/debito
                tecbank_db.REMOVE<BankCard>("cards",bc => bc.card_num == card_num);
            } catch (DBMSException e1 ){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2 ){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Update and modify the information a bank card
        /// </summary>
        /// <param name="card"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        public void Card_Update(BankCard card){
            // >> Verificar valiz del objeto
            if (card == null)
                throw new ArgumentNullException($"(TECBANKSERVICE) ({nameof(card)}) object is null and therefore not valid in the database");
            try{
                // >> Obtener la version actual del objeto en la tabla
                var existingCard = tecbank_db.SELECT<BankCard>("cards", bc  => bc.card_num == card.card_num);
                if (existingCard == null)
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Bank card(ID={card.card_num}) not found");

                tecbank_db.MODIFY<BankCard>("cards", card, (a,b) => a.card_num == b.card_num);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        // ::. EMPLOYEE METHODS

        /// <summary>
        /// Retrieves all bank employees from the database
        /// </summary>
        /// <returns>List of employees or empty list</returns>
        public List<BankEmployee> GetAllEmployees() {
            try {
                var employees = tecbank_db.SELECT<BankEmployee>("employees");
                return employees;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Retrives all loan adviser
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public List<BankEmployee> GetAllLoanAdvisers(){
            // Get all employees with role_id = 2 (loan advisers)
            try {
                var advisers = tecbank_db.SELECT<BankEmployee>("employees",  e => e.role_id == 2);
                return advisers;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Finds and returns a bank employee with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Object of a bank employee or null if not found</returns>
        /// <exception cref="ServiceException"></exception>
        public BankEmployee? Employee_GetById(int id){
            try {
                var employee = tecbank_db.SELECT<BankEmployee>("employees", e => e.id == id).FirstOrDefault();
                return employee;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Finds and returns a loan adviser with the given id
        /// </summary>
        /// <returns> Object of an adviser employee or null if not found</returns>
        /// <exception cref="ServiceException"></exception>
        public BankEmployee? Adviser_GetById(int id){
            try {
                var adviser = tecbank_db.SELECT<BankEmployee>("employees", e => e.id == id && e.role_id == 2).FirstOrDefault();
                return adviser;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public void Employee_Add(BankEmployee employee){
            if (employee == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(employee)} is null and therefore not a valid object in the database");
            try{
                // >> Verify the selected role is valid
                var emp_role = tecbank_db.SELECT<Role>("roles",e => e.id == employee.role_id).FirstOrDefault() ??
                    throw new InvalidOperationException($"(TECBANKSERVICE) Role(ID={employee.id}) from employee is not a valid role");
                // >> Insert the employee
                tecbank_db.INSERT<BankEmployee>("employees", employee);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        public void Employee_Update(BankEmployee employee){
            // >> Verificar valiz del objeto
            if (employee == null)
                throw new ArgumentNullException($"(TECBANKSERVICE) ({nameof(employee)}) object is null and therefore not valid in the database");
            try{
                // >> Verify the selected role is valid
                var emp_role = tecbank_db.SELECT<Role>("roles",e => e.id == employee.role_id).FirstOrDefault() ??
                    throw new InvalidOperationException($"(TECBANKSERVICE) Role(ID={employee.id}) from employee is not a valid role");
                // >> Obtener la version actual del objeto en la tabla
                var existingEmployee = tecbank_db.SELECT<BankEmployee>("employees", e => e.id == employee.id);
                if (existingEmployee == null)
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Employee(ID={employee.id}) not found");

                tecbank_db.MODIFY<BankEmployee>("employees", employee, (a,b) => a.id == b.id);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public void Employee_Delete(int employee_id){
            try{
                // >> Removeer el empleado de la base de datos
                tecbank_db.REMOVE<BankEmployee>("employees", e => e.id == employee_id);
            } catch (DBMSException e1 ){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2 ){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        // ::. LOAN PAYMENT METHODS

        /// <summary>
        /// Retrieves all loan payments from the database
        /// </summary>
        /// <exception cref="ServiceException"></exception>
        /// <returns>List of payments or empty list on error</returns>
        public List<LoanPayment> GetAllLoanPayments() {
            try {
                var payments = tecbank_db.SELECT<LoanPayment>("payments");
                return payments;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }
        
        /// <summary>
        /// Finds all the loan payments for client
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<LoanPayment> LoanPayments_FromClient(int user_id) {
            try{
                // >> Buscar cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                // >> Buscar todos prestamos del cliente
                var loans = tecbank_db.SELECT<BankLoan>("loans", ln => ln.client_id == user_id);
                // >> Agregar todos los pagos para cada prestamo
                List<LoanPayment> payments = [];
                foreach(var loan in loans){
                    payments = payments.Concat(tecbank_db.SELECT<LoanPayment>("payments",p => p.loan_id == loan.id)).ToList();
                }
                return payments;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Get the scheduled payments for a loan
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="loan_id"></param>
        /// <returns>List of loan payments from the schedule of a loan</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<LoanPayment> LoanPayments_FromLoan(int client_id, int loan_id) {
            try{
                // >> Buscar el cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == client_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={client_id}) not found in database");
                // >> Buscar el prestamo del cliente
                var loan = tecbank_db.SELECT<BankLoan>("loans", ln => ln.id == loan_id && ln.client_id == client_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Loan(ID={loan_id}) from client(ID={client_id}) doesn't exist on the database");
                // >> Buscar los pagos relacionados al prestamo
                var payments = tecbank_db.SELECT<LoanPayment>("payments",p => p.loan_id == loan.id && p.type == 1);
                return payments;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Makes a payment for a loan on databse, either for a scheduled one or for extraordinary one
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="account_id"></param>
        /// <param name="payment"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// Extraordinary payments are payments outside of the schedule; meanwhile Ordinary payments are those from the schedule
        /// </remarks>
        public void Payment_MakeAPayment(int user_id, String account_id, LoanPayment payment){
            if (payment == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(payment)} is null and therefore not a valid object in the database");
            try{
                payment.state = 1;
                // >> Verificar la existencia del cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");

                // >> Verificar la existencia de la cuenta bancaria
                var account = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.client_id == client.id && ba.id == account_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={account_id}) not found in database");
                // >> Verificar que la cuenta tenga suficiente dinero para hacer el pago
                if (account.balance < payment.total)
                    throw new InvalidOperationException($"(TECBANKSERVICE) Bank account(ID={account_id}) doesn't have enough money to make a movement");

                // >> Buscar el prestamo al que se desea pagar y actualizarlo
                var loan = tecbank_db.SELECT<BankLoan>("loans", ln => ln.id == payment.loan_id).FirstOrDefault()??
                    throw new ArgumentException($"(TECBANKSERVICE) Loan(ID={payment.loan_id}) doesn't exist on database");
                loan.balance -= payment.total;
                if (loan.balance <= 0){
                    loan.balance = 0;
                    loan.state = 1;
                }
                tecbank_db.MODIFY<BankLoan>("loans",loan,(a,b) => a.id == b.id);

                // >> Obtener el tipo de moneda en uso para conversiones adecuadas
                var account_currency = tecbank_db.SELECT<Currency>("currency", cur => cur.id == account.currency_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={account.currency_id}) from account(ID={account.id}) doesn't exist on database");
                var transaction_currency = tecbank_db.SELECT<Currency>("currency", cur => cur.id == loan.currency_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={loan.currency_id}) from loan(ID={loan.id}) doesn't exist on database");
                // >> Registar un nuevo movimiento bancario relacionado al prestamo y con la cuenta indicada
                BankMovement related_movement = new BankMovement{id = Guid.NewGuid().ToString(), description= "Pago de prestamo", date = DateTime.Now, card_id = -1, total_transfer = -payment.total, currency_id = loan.currency_id, account_id = account.id, type = 3};
                tecbank_db.INSERT<BankMovement>("movements",related_movement);
                account.balance = (related_movement.total_transfer*transaction_currency.usd_exchange)/account_currency.usd_exchange;
                tecbank_db.MODIFY<BankAccount>("accounts",account,(a,b) => a.id == b.id);

                // >> Identificar si el pago es de tipo extraordinario o ordinario (segun el id)
                payment.movement_id = related_movement.id;
                var existingPayment = tecbank_db.SELECT<LoanPayment>("payments", p => p.id == payment.id).FirstOrDefault();
                if (existingPayment == null){ // Pago extraordinario => añadir un nuevo pago
                    payment.type = 2;
                    payment.date = DateTime.Now;
                    tecbank_db.INSERT<LoanPayment>("payments", payment);
                    var other_payments = tecbank_db.SELECT<LoanPayment>("payments", p => p.id != payment.id && p.state == 0);
                    foreach(var pm in other_payments){
                        pm.total = (loan.balance/other_payments.Count)+(loan.total*loan.interest_rate)/100;
                        tecbank_db.MODIFY<LoanPayment>("payments",pm,(a,b)=> a.id == b.id);
                    }
                } else { // Pago ordinario => modificar el pago del calendario existente
                    payment.type = 1;
                    payment.date = existingPayment.date;
                    tecbank_db.MODIFY<LoanPayment>("payments",payment,(a,b)=>a.id == b.id); 
                }
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        // ::. BANK LOAN METHODS
        
        /// <summary>
        /// Retrieves all loan payments from the database
        /// </summary>
        /// <exception cref="ServiceException"></exception>
        /// <returns>List of payments or empty list on error</returns>
        public List<BankLoan> GetAllLoans(){
            try {
                var loans = tecbank_db.SELECT<BankLoan>("loans");
                return loans;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        /// <summary>
        /// Get the information on a specific loan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public BankLoan? Loan_Get(int id){
            try {
                var loan = tecbank_db.SELECT<BankLoan>("loans", ln => ln.id == id).FirstOrDefault();
                return loan;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Get all the loans from a client
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<BankLoan> Loans_FromClient(int user_id){
            try{
                // >> Buscar cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                // >> Buscar todos prestamos del cliente
                var loans = tecbank_db.SELECT<BankLoan>("loans", ln => ln.client_id == user_id);
                return loans;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        /// <summary>
        /// Create a new loan in the database binded to a client and an adviser
        /// </summary>
        /// <param name="loan"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Loan_Add(BankLoan loan){
            if (loan == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(loan)} is null and therefore not a valid object in the database");
            try{ 
                // >> Validar datos del prestamo
                if (loan.total <= 0)
                    throw new ArgumentException("(TECBANKSERVICE) Loan amount must be positive");
                // >> Valores por defecto
                loan.balance = loan.total;
                loan.request_date = DateTime.Now;
                // >> Comprobar el tipo de dinero para el prestamo
                var loan_cur = tecbank_db.SELECT<Currency>("currency", c => c.id == loan.currency_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={loan.currency_id}) used for loan(ID={loan.id}) doesn't exist in database");
                // >> Insertar el nuevo prestamo
                tecbank_db.INSERT<BankLoan>("loans", loan);
                // >> Calendarizar los pagos del prestamo ordinarios
                float per_pay = (loan.balance/loan.lapse)+(loan.total*loan.interest_rate)/100;
                DateTime today = loan.request_date;
                DateTime concurrent = new DateTime(today.Year, today.Month, today.Day, 23, 59, 0);
                for (int i = 0; i < loan.lapse; i++){
                    var new_payment = new LoanPayment{ id = Guid.NewGuid().ToString(), loan_id = loan.id, total = per_pay, movement_id = "", type=1, state=0, date = concurrent.AddMonths(i+1)};
                    tecbank_db.INSERT<LoanPayment>("payments",new_payment);
                }
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        /// <summary>
        /// Update and modify the information on a loan
        /// </summary>
        /// <param name="loan"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        public void Loan_Update(BankLoan loan){
            try{
                // >> Verificar valiz del objeto
                if (loan == null)
                    throw new ArgumentNullException($"(TECBANKSERVICE) Loan({nameof(loan)}) object is null");
                // >> Obtener la version actual del objeto en la tabla
                var existingLoan = tecbank_db.SELECT<BankLoan>("loans", ln  => ln.id == loan.id);
                if (existingLoan == null)
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Loan(ID={loan.id}) not found");

                tecbank_db.MODIFY<BankLoan>("loans", loan, (a,b) => a.id == b.id);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        // ::. BANK MOVEMENT METHODS

        /// <summary>
        /// Get all the movements in the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public List<BankMovement> GetAllMovements() {
            try {
                var movs = tecbank_db.SELECT<BankMovement>("movements");
                return movs;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public BankMovement? Movement_Get(String mov_id){
            try {
                var movement = tecbank_db.SELECT<BankMovement>("movements", m => m.id == mov_id).FirstOrDefault();
                return movement;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }


        /// <summary>
        /// Makes a bank movement from an account to another account.
        /// </summary>
        /// <param name="sender_id"></param>
        /// <param name="receiver_id"></param>
        /// <param name="movement"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// If the movement includes a debit/credit card, it will be binded to the movement
        /// </remarks>
        public void Movement_New_AccountToAccount(String sender_id, String receiver_id, BankMovement movement){
            if (movement == null)
                throw new ArgumentNullException($"(TECBANKSERBIVE) {nameof(movement)} is null and therefore is not a valid object in the database");
            try{
                movement.date = DateTime.Now;
                // >> Comprobar existencia de ambas cuentas
                var accs = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.id == sender_id || ba.id == receiver_id);
                var sender_acc = accs.FirstOrDefault(acc => acc.id == sender_id)??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={sender_id}) for sender doesn't exist on the database");
                var receiver_acc = accs.FirstOrDefault(acc => acc.id == receiver_id)??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={receiver_id}) for receiver doesn't exist on the database");
                // >> Obtener los tipos de dinero utilizados
                var currencies = tecbank_db.SELECT<Currency>("currency",cur => cur.id == movement.currency_id || cur.id == sender_acc.currency_id || cur.id == receiver_acc.currency_id);
                var mov_cur = currencies.FirstOrDefault(mv => mv.id == movement.currency_id) ??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={movement.currency_id}) from new movement doesn't exist on database");
                var send_cur = currencies.FirstOrDefault(mv => mv.id == sender_acc.currency_id)??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={sender_acc.currency_id}) from sender bank account(ID={sender_id}) doesn't exist on database");
                var recv_cur = currencies.FirstOrDefault(mv => mv.id == receiver_acc.currency_id)??
                    throw new ArgumentException($"(TECBANKSERVICE) Currency(ID={sender_acc.currency_id}) from receiver bank account(ID={receiver_id}) doesn't exist on database");
                // >> Mostrar cambios a las cuentas respectivas
                sender_acc.balance -= (movement.total_transfer*mov_cur.usd_exchange)/send_cur.usd_exchange;
                receiver_acc.balance += (movement.total_transfer*mov_cur.usd_exchange)/recv_cur.usd_exchange;
                tecbank_db.MODIFY<BankAccount>("accounts",sender_acc,(a,b)=>a.id == b.id);
                tecbank_db.MODIFY<BankAccount>("accounts",receiver_acc,(a,b)=>a.id == b.id);
                // >> Agregar el nuevo movimiento a la base de datos
                movement.id = Guid.NewGuid().ToString();
                movement.total_transfer = movement.total_transfer;
                movement.account_id = receiver_acc.id;
                tecbank_db.INSERT<BankMovement>("movements",movement);

                movement.id = Guid.NewGuid().ToString();
                movement.total_transfer = -movement.total_transfer;
                movement.account_id = sender_acc.id;
                tecbank_db.INSERT<BankMovement>("movements",movement);
            } catch (KeyNotFoundException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (DBMSException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        /// <summary>
        /// Makes a movement with a card, such as credit debt payments or ATM withdrawals
        /// </summary>
        /// <param name="card_num"></param>
        /// <param name="movement"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Movement_New_WithCard(int card_num, BankMovement movement){
            if (movement == null)
                throw new ArgumentNullException($"(TECBANKSERBIVE) {nameof(movement)} is null and therefore is not a valid object in the database");
            try{
                movement.id = Guid.NewGuid().ToString();
                movement.date = DateTime.Now;
                // >> Verificar que exista la tarjeta designada
                var card = tecbank_db.SELECT<BankCard>("cards",bc => bc.card_num == card_num).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank card(ID={card_num}) doesn't exist on database");
                card.balance += movement.total_transfer;
                movement.card_id = card.card_num;
                // >> Verificar que exista una cuenta asociada a la tarjeta
                var account = tecbank_db.SELECT<BankAccount>("accounts",ba => ba.id == card.account_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={card.account_id}) from bank card(ID={card_num}) doesn't exist on database");
                account.balance -= Math.Abs(movement.total_transfer);
                movement.account_id = account.id;
                // >> Aplicar cambios a la tarjeta y la cuenta
                tecbank_db.MODIFY<BankCard>("cards",card, (a,b)=>a.card_num == b.card_num);
                tecbank_db.MODIFY<BankAccount>("accounts",account, (a,b)=>a.id == b.id);
                // >> Registrar el movimiento
                tecbank_db.INSERT<BankMovement>("movements",movement);
            } catch (KeyNotFoundException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (DBMSException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        /// <summary>
        /// Retrieves all movements from a bank account 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="client_id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ServiceException"></exception>
        public List<BankMovement> Movements_FromAccount(String account_id, int client_id){
            try{
                // >> Buscar cliente
                var account = tecbank_db.SELECT<BankAccount>("accounts",ba => ba.id == account_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={account_id}) not found in database");
                // >> Buscar todos prestamos del cliente
                var movs = tecbank_db.SELECT<BankMovement>("movements", mov => mov.account_id == account.id);
                return movs;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }
        
    }
}