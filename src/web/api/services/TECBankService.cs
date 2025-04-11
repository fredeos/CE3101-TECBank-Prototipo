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
        // --------------------------------[Temporary simulated entities (TODO: replace with actual database access point)]
        private static List<ClientAccount> clients = new List<ClientAccount>
        {
            new ClientAccount {id = 1, name = "Isaac", last_name1 = "Ramirez", last_name2="Herrera", type = 1, username = "snk100", password = "1234", monthly_income = 10000000, phone_number = "+506 3163 7168", address = "Cartago"},
            new ClientAccount {id = 2, name = "Marco", last_name1 = "Rivera", last_name2="Meneses", type = 1, username = "marc300", password = "1234", monthly_income = 6300000, phone_number = "+506 8163 7268", address = "San Jose"}
        }; 

        private static List<BankAccount> accounts = new List<BankAccount>{
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

        private static List<Employee> employees = new List<Employee>{
            new Employee { id=7, name="Juan", last_name1="Miranda", last_name2="Solis", role_id=1},
            new Employee { id=9, name="Adolfo", last_name1="Vargas", last_name2="Paniagua", role_id=2},
            new Employee { id=7, name="Daniel", last_name1="Cabrera", last_name2="Ortiz", role_id=2}
        };
        // --------------------------------[ Service atributes and properties]--------------------------------
        private static readonly String db_file = "tecbank";
        private static DBConnect tecbank_db = new DBConnect(db_file);
        // --------------------------------[ Service functions and methods ]--------------------------------

        // ::. CLIENT METHODS
        public List<ClientAccount> GetAllClients(){
            try {
                var clients = tecbank_db.SELECT<ClientAccount>("clients");
                return clients;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public ClientAccount? Client_findByID(int id) {
            try {
                var client = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == id).FirstOrDefault();
                return client;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public ClientAccount? Client_find(String username, String password){
            try {
                var client = tecbank_db.SELECT<ClientAccount>("clients", c => c.username == username && c.password == password).FirstOrDefault();
                return client;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

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
        public List<BankAccount> GetAllAccounts() {
            try {
                var accounts = tecbank_db.SELECT<BankAccount>("accounts");
                return accounts;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public BankAccount? Account_Get(string id){
            try {
                var acc = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.id == id).FirstOrDefault();
                return acc;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public List<BankAccount> Accounts_FromClient(int user_id){
            try{
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                var accounts = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.client_id == client.id);
                return accounts;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public void Account_Add(BankAccount account){
            if (account == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(account)} is null and therefore not a valid object in the database");
            try{
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == account.client_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={account.client_id}) from give bank account(ID={account.id}) doesn't exist in the database");
                tecbank_db.INSERT<BankAccount>("accounts", account);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        public void Account_Update(BankAccount account){
            try{
                if (account == null)
                    throw new ArgumentNullException($"(TECBANKSERVICE) Bank account({nameof(account)}) object is null");
                
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

        public void Account_Delete(String id){
            try{
                // >> Buscar la cuenta bancaria indicada
                var acc = tecbank_db.SELECT<BankAccount>("clients", ba => ba.id == id).FirstOrDefault() ??
                    throw new KeyNotFoundException($"(TECBANKSERVICE) Bank account(ID={id}) not found in the database");
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
        public List<BankCard> GetAllCards(){
            try {
                var cards = tecbank_db.SELECT<BankCard>("cards");
                return cards;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public BankCard? Card_Get(int card_num){
            try {
                var card = tecbank_db.SELECT<BankCard>("cards", bc => bc.card_num == card_num).FirstOrDefault();
                return card;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

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

        public void Card_Add(BankCard card){
            if (card == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(card)} is null and therefore not a valid object in the database");
            try{
                var account = tecbank_db.SELECT<BankAccount>("accounts", ba => ba.id == card.account_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Bank account(ID={card.account_id}) used for card(ID={card.card_num}) doesn't exist in database");
                tecbank_db.INSERT<BankCard>("cards", card);
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            } catch (ArgumentException e3){
                throw new ArgumentException($"(TECBANKSERVICE){e3.ToString()}");
            }
        }

        
        public void Card_Delete(int cardNum){
            try{
                // >> Remover la tarjeta de credito
                tecbank_db.REMOVE<BankCard>("cards",bc => bc.card_num == cardNum);
            } catch (DBMSException e1 ){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            } catch (KeyNotFoundException e2 ){
                throw new ServiceException($"(TECBANKSERVICE){e2.ToString()}");
            }
        }

        public void Card_Update(BankCard card){
            try{
                // >> Verificar valiz del objeto
                if (card == null)
                    throw new ArgumentNullException($"(TECBANKSERVICE) Bank card({nameof(card)}) object is null");
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
        public List<Employee> GetAllEmployees() {
            try {
                var employees = tecbank_db.SELECT<Employee>("employees");
                return employees;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        // ::. LOAN PAYMENT METHODS
        public List<LoanPayment> GetAllPayments() {
            try {
                var payments = tecbank_db.SELECT<LoanPayment>("payments");
                return payments;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public List<LoanPayment> Payments_FromClient(int user_id) {
            try{
                // >> Buscar cliente
                var client = tecbank_db.SELECT<ClientAccount>("clients",c => c.id == user_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={user_id}) not found in database");
                // >> Buscar todos prestamos del cliente
                var loans = tecbank_db.SELECT<BankLoan>("loans", ln => ln.client_id == user_id);
                // >> Agregar todos los pagos prestamo para cada prestamo
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
                BankMovement related_movement = new BankMovement{id = Guid.NewGuid().ToString(), description= "Pago de prestamo", date = payment.date, card_id = -1, total_transfer = -payment.total, currency_id = loan.currency_id, account_id = account.id, type = 3};
                tecbank_db.INSERT<BankMovement>("movements",related_movement);
                account.balance = (related_movement.total_transfer*transaction_currency.usd_exchange)/account_currency.usd_exchange;
                tecbank_db.MODIFY<BankAccount>("accounts",account,(a,b) => a.id == b.id);

                // >> Identificar si el pago es de tipo extraordinario o ordinario (segun el id)
                payment.movement_id = related_movement.id;
                var existingPayment = tecbank_db.SELECT<LoanPayment>("payments", p => p.id == payment.id).FirstOrDefault();
                if (existingPayment == null){ // Pago extraordinario => añadir un nuevo pago
                    payment.type = 2;
                    tecbank_db.INSERT<LoanPayment>("payments", payment);
                    var other_payments = tecbank_db.SELECT<LoanPayment>("payments", p => p.id != payment.id && p.state == 0);
                    foreach(var pm in other_payments){
                        pm.total = (loan.balance/other_payments.Count)+(loan.total*loan.interest_rate)/100;
                        tecbank_db.MODIFY<LoanPayment>("payments",pm,(a,b)=> a.id == b.id);
                    }
                } else { // Pago ordinario => modificar el pago del calendario existente
                    payment.type = 1;
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
        public List<BankLoan> GetAllLoans(){
            try {
                var loans = tecbank_db.SELECT<BankLoan>("loans");
                return loans;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

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

        public void Loan_Add(BankLoan loan){
            if (loan == null) 
                throw new ArgumentNullException($"(TECBANKSERVICE) {nameof(loan)} is null and therefore not a valid object in the database");
            try{
                // >> Comprobar existencia del cliente y el prestamista
                var client = tecbank_db.SELECT<ClientAccount>("clients", c => c.id == loan.client_id).FirstOrDefault() ??
                    throw new ArgumentException($"(TECBANKSERVICE) Client(ID={loan.client_id}) used for loans(ID={loan.id}) doesn't exist in database");
                var adviser = tecbank_db.SELECT<Employee>("employees", em => em.id == loan.adviser_id && em.role_id == 2) ??
                    throw new ArgumentException($"(TECBANKSERVICE) Loan adviser(ID={loan.adviser_id}) doesn't exist on the database");
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

        public void Loan_Update(BankLoan loan){
            // if (loan == null)
            //     throw new ArgumentNullException(nameof(loan));

            // var existingLoan = loans.FirstOrDefault(l => l.id == loan.id);
            // if (existingLoan == null)
            //     throw new KeyNotFoundException("Préstamo no encontrado.");

            // // Solo actualiza campos modificables (evita cambiar client_id o adviser_id)
            // existingLoan.lapse = loan.lapse;
            // existingLoan.interest_rate = loan.interest_rate;
            // existingLoan.balance = loan.balance;
            // existingLoan.total = loan.total;
            // existingLoan.state = loan.state;
        }

        // ::. BANK MOVEMENT METHODS
        public List<BankMovement> GetAllMovements() {
            try {
                var movs = tecbank_db.SELECT<BankMovement>("movements");
                return movs;
            } catch (DBMSException e1){
                throw new ServiceException($"(TECBANKSERVICE){e1.ToString()}");
            }
        }

        public void Movement_New(int user_id, BankMovement movement) {
            // // >> COMPROBACION 1: Integridad del objeto <<
            // if (movement == null) throw new ArgumentNullException(nameof(movement));
            // var user_accounts = accounts.FindAll(acc => acc.client_id == user_id);

            // // >> COMPROBACION 2: Existencia de cuentas <<
            // if (user_accounts.Count == 0) throw new KeyNotFoundException("El usuario tramitante no tiene ninguna cuenta a su nombre");
            // bool acc_exists = false;
            // for (int i = 0; i < user_accounts.Count; i++){
            //     if (user_accounts[i].id == movement.account_id){
            //         acc_exists = true;
            //         break;
            //     }
            // }
            // // >> COMPROBACION 3: Movimiento.Cuenta corresponde Usuario.Cuenta <<
            // if (!acc_exists) throw new KeyNotFoundException("La cuenta asignada al movimiento no corresponde a ninguna cuenta del cliente");
            // movement.id = Guid.NewGuid().ToString();
            // movements.Add(movement);
        }

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