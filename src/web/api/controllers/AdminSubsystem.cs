using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tecbank.models;
using tecbank.services;
using tecbank.services.logger;

namespace tecbank.controllers{
    [Route("services/admin")]
    [ApiController]
    public class Admin : ControllerBase {
        private readonly TECBankService tecbankService;
        private readonly LogService logService;
        private readonly PasswordService security;
        private readonly int password;
        public Admin(TECBankService service, LogService log, PasswordService pass){
            this.tecbankService = service;
            this.logService = log;
            this.security = pass;
            this.password = security.AdminPassKey;
        }

        // ------------------------------------------------- [ General GET ] -------------------------------------------------
        [HttpGet("login/{key}")]
        public ActionResult Login(int key){
            if (key != password){
                return StatusCode(500,"Access key for administrator access is invalid");
            } else {
                return Ok();
            }
        }

        [HttpGet("{key}/clients/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetClients(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetClients)}) Clients retrieved succesfully");
                return Ok(tecbankService.GetAllClients());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetClients)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/accounts/all")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAccounts)}) Bank accounts retrieved succesfully");
                return Ok(tecbankService.GetAllAccounts());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccounts)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/cards/all")]
        public ActionResult<IEnumerable<BankCard>> GetCards(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCards)}) Bank cards retrieved succesfully");
                return Ok(tecbankService.GetAllCards());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCards)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/employees/all")]
        public ActionResult<IEnumerable<BankEmployee>> GetEmployees(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetEmployees)}) Employees retrieved succesfully");
                return Ok(tecbankService.GetAllEmployees());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetEmployees)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/employees/advisers/all")]
        public ActionResult<IEnumerable<BankEmployee>> GetAdvisers(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAdvisers)}) Employees retrieved succesfully");
                return Ok(tecbankService.GetAllLoanAdvisers());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAdvisers)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/movements/all")]
        public ActionResult<IEnumerable<BankMovement>> GetMovements(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetMovements)}) Bank movements retrieved succesfully");
                return Ok(tecbankService.GetAllMovements());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetMovements)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/loans/all")]
        public ActionResult<IEnumerable<BankLoan>> GetLoans(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
           try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetEmployees)}) Loans retrieved succesfully");
                return Ok(tecbankService.GetAllLoans());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetEmployees)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/loans/payments/all")]
        public ActionResult<IEnumerable<LoanPayment>> GetLoanPayments(int key){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoanPayments)}) Loan payments retrieved succesfully");
                return Ok(tecbankService.GetAllLoanPayments());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoanPayments)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{key}/roles/all")]
        public ActionResult<IEnumerable<Role>> GetRoles(int key)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            try
            {
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetRoles)}) Roles retrieved successfully");
                return Ok(tecbankService.GetAllRoles());
            }
            catch (ServiceException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetRoles)}){e1.ToString()}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{key}/goals/all")]
        public ActionResult<IEnumerable<AdviserGoal>> GetAllAdviserGoals(int key)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            try
            {
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAllAdviserGoals)}) Goals retrieved successfully");
                return Ok(tecbankService.GetAllAdviserGoals());
            }
            catch (ServiceException e)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAllAdviserGoals)}){e.ToString()}");
                return StatusCode(500, "Internal server error");
            }
        }
        

        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------
        [HttpGet("{key}/clients/{id}")]
        public ActionResult<ClientAccount> GetClient(int key, int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var client = tecbankService.Client_findByID(id);
                if (client == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetClient)}) No matching data was found in the database for client(ID={id})");
                    return NotFound($"Client(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetClient)}) Client(ID={id}) was found successfully");
                return Ok(client);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetClient)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
            
        }

        [HttpGet("{key}/accounts/{id}")]
        public ActionResult<BankAccount> GetAccount(int key, string id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var acc = tecbankService.Account_Get(id);
                if (acc == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAccount)}) No matching data was found in the database for account(ID={id})");
                    return NotFound($"Bank account(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAccount)}) Bank account(ID={id}) was found successfully");
                return Ok(acc);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccount)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/cards/{id}")]
        public ActionResult<BankCard> GetCard(int key,int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var card = tecbankService.Card_Get(id);
                if (card == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetCard)}) No matching data was found in the database for card(ID={id})");
                    return NotFound($"Card(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCard)}) Bank card(ID={id}) was found successfully");
                return Ok(card);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCard)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/employees/{id}")]
        public ActionResult<BankEmployee> GetEmployee(int key, int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var employee = tecbankService.Employee_GetById(id);
                if (employee == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetEmployee)}) No matching data was found in the database for employee(ID={id})");
                    return NotFound($"Employee(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetEmployee)}) Employee(ID={id}) was found successfully");
                return Ok(employee);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetEmployee)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/employees/advisers/{id}")]
        public ActionResult<BankEmployee> GetAdviser(int key,int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var adviser = tecbankService.Adviser_GetById(id);
                if (adviser == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAdviser)}) No matching data was found in the database for employee(ID={id})");
                    return NotFound($"Employee(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAdviser)}) Employee(ID={id}) was found successfully");
                return Ok(adviser);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAdviser)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/loans/{id}")]
        public ActionResult<BankLoan> GetLoan(int key, int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var loan = tecbankService.Loan_Get(id);
                if (loan == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetLoan)}) No matching data was found in the database for loan(ID={id})");
                    return NotFound($"Loan(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoan)}) Bank loan(ID={id}) was found successfully");
                return Ok(loan);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoan)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/movements/{id}")]
        public ActionResult<BankMovement> GetMovement(int key, String id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                var movement = tecbankService.Movement_Get(id);
                if (movement == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetMovement)}) No matching data was found in the database for movement(ID={id})");
                    return NotFound($"Movement(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetMovement)}) Bank movement(ID={id}) was found successfully");
                return Ok(movement);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetMovement)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("{key}/goals/adviser/{adviserId}")]
        public ActionResult<IEnumerable<AdviserGoal>> GetAdviserGoals(int key, int adviserId)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            try
            {
                var adviser = tecbankService.GetAdviserGoals(adviserId);
                if (adviser.Count == 0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAdviser)}) No matching data was found in the database for adviser(ID={adviserId})");
                    return NotFound($"Adviser(ID={adviserId}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAdviserGoals)}) Goals for adviser {adviserId} retrieved successfully");
                return Ok(adviser);
            }
            catch (ServiceException e)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAdviserGoals)}){e.ToString()}");
                return StatusCode(500, "Internal server error");
            }
        }

        // ------------------------------------------------- [ POST ] -------------------------------------------------

        [HttpPost("{key}/roles/add")]
        public ActionResult<Role> AddRole(int key, [FromBody] Role role)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            try
            {
                // Basic validation
                if (role.id <= 0)
                {
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddRole)}) Role ID must be positive");
                    return BadRequest("Role ID must be positive");
                }
                
                if (string.IsNullOrWhiteSpace(role.name))
                {
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddRole)}) Role name cannot be empty");
                    return BadRequest("Role name cannot be empty");
                }

                tecbankService.AddRole(role);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(POST={nameof(AddRole)}) Role(ID={role.id}) was added successfully");
                return CreatedAtAction(nameof(AddRole), new { id = role.id }, role);
            }
            catch (ServiceException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddRole)}){e1.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            catch (ArgumentNullException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddRole)}){e2.ToString()}");
                return BadRequest("Role object doesn't have a valid format");
            }
            catch (ArgumentException e3)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddRole)}){e3.ToString()}");
                return BadRequest(e3.Message);
            }
        }
        
        [HttpPost("{key}/clients/add")]
        public ActionResult AddClient(int key, [FromBody] ClientAccount client){
            try{
                if (client.id <= 0){
                    logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddClient)}) Client.id is negative and therefore can't be added to the database");
                    return BadRequest("Client.id must be positive");
                }
                tecbankService.Client_Add(client);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddClient)}) Client(ID={client.id}) was added successfully");
                return CreatedAtAction(nameof(AddClient),new {id = client.id},client);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddClient)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddClient)}){e2.ToString()}");
                return BadRequest("Client object doesn't have a valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddClient)}){e3.ToString()}");
                return BadRequest("Failed to add client to database");
            }
        }

        [HttpPost("{key}/accounts/add")]
        public ActionResult<BankAccount> AddAccount(int key, [FromBody] BankAccount account){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                // >> Verificar existencia del cliente
                var owner = tecbankService.Client_findByID(account.client_id);
                if (owner == null){
                    logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddAccount)}) Bank account(ID={account.id}) doesn't belong to a known client");
                    return BadRequest($"Account owner client(ID={account.client_id}) doesn't exist in the database");
                }
                // >> Agregar cuenta
                tecbankService.Account_Add(account);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddAccount)}) Bank account(ID={account.id}) was added successfully");
                return CreatedAtAction(nameof(AddAccount), new {id = account.id}, account);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddAccount)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddAccount)}){e2.ToString()}");
                return BadRequest("Bank account object doesn't have a valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddAccount)}){e3.ToString()}");
                return BadRequest("Failed to add banck account to database");
            }
        }

        [HttpPost("{key}/cards/add")]
        public ActionResult<BankCard> AddCard(int key, [FromBody] BankCard card){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                // >> Comprobar existencia de la cuenta dueña
                var account = tecbankService.Account_Get(card.account_id);
                if (account == null){
                    logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddCard)}) Bank account(ID={card.account_id}) from card doesn't exist on the database");
                    return BadRequest($"(HTTP)(POST={nameof(AddCard)}) Bank account(ID={card.account_id}) doesn't exist on the database");
                }
                // >> Agregar el prestamo
                tecbankService.Card_Add(card);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddCard)}) Card(ID={card.card_num}) successfully added to database");
                return CreatedAtAction(nameof(AddCard), new { id = card.card_num }, card);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddCard)}){e1.ToString()}");
                return BadRequest("Card object is null");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddCard)}){e2.ToString()}");
                return BadRequest("Credit/debit card content is invalid");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddCard)}){e3.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpPost("{key}/loans/add")]
        public ActionResult<BankLoan> AddLoan(int key, [FromBody] BankLoan loan){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                // >> Comprobar existencia del cliente
                var client = tecbankService.Client_findByID(loan.client_id);
                if (client == null){
                    logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddLoan)}) Client(ID={loan.client_id}) from loan doesn't exist on the database");
                    return BadRequest($"(HTTP)(POST={nameof(AddLoan)}) Client(ID={loan.client_id}) doesn't exist on the database");
                }
                // >> Comprobar existencia del prestamista
                var adviser = tecbankService.Adviser_GetById(loan.adviser_id);
                if (client == null){
                    logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddLoan)}) Loan adviser(ID={loan.adviser_id}) from loan doesn't exist on the database");
                    return BadRequest($"(HTTP)(POST={nameof(AddLoan)}) Client(ID={loan.adviser_id}) doesn't exist on the database");
                }
                // >> Agregar el prestamo
                tecbankService.Loan_Add(loan);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddLoan)}) Loan(ID={loan.id}) successfully added to database");
                return CreatedAtAction(nameof(AddLoan), new { id = loan.id }, loan);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddLoan)}){e1.ToString()}");
                return BadRequest("Loan object is null");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddLoan)}){e2.ToString()}");
                return BadRequest("Loan object content is invalid");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddLoan)}){e3.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpPost("{key}/employees/add")]
        public ActionResult<BankLoan> AddEmployee(int key, [FromBody] BankEmployee employee){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                // >> Agregar el prestamo
                tecbankService.Employee_Add(employee);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddEmployee)}) Employee(ID={employee.id}) successfully added to database");
                return CreatedAtAction(nameof(AddEmployee), new { id = employee.id }, employee);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddEmployee)}){e1.ToString()}");
                return BadRequest("Loan object is null");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddEmployee)}){e2.ToString()}");
                return BadRequest("Loan object content is invalid");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddEmployee)}){e3.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpPost("{key}/goals/add")]
        public ActionResult<AdviserGoal> AddAdviserGoal(int key, [FromBody] AdviserGoal goal)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            try
            {
                // Basic validation
                if (goal.adviser_id <= 0)
                {
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddAdviserGoal)}) Goal ID must be positive");
                    return BadRequest("Goal ID must be positive");
                }

                tecbankService.AddAdviserGoal(goal);
                
                logService.Log_New(LogTypes.INFO, $"(HTTP)(POST={nameof(AddAdviserGoal)}) Goal(ID={goal.adviser_id}) for adviser(ID={goal.adviser_id}) was added successfully");
                return CreatedAtAction(nameof(AddAdviserGoal), new { id = goal.adviser_id }, goal);
            }
            catch (ServiceException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddAdviserGoal)}){e1.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            catch (ArgumentNullException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddAdviserGoal)}){e2.ToString()}");
                return BadRequest("Goal object doesn't have a valid format");
            }
            catch (ArgumentException e3)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(AddAdviserGoal)}){e3.ToString()}");
                return BadRequest(e3.Message);
            }
        }

        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        [HttpPut("{key}/clients/update/{id}")]
        public ActionResult<ClientAccount> UpdateClient(int key, int id, [FromBody] ClientAccount client){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            if (id != client.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateClient)}) Client ID({id}) doesnt match body ID({client.id})");
                return BadRequest($"Client ID({id}) doesnt match body ID({client.id})");
            }
            try{
                tecbankService.Client_Update(client);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateClient)}) Client(ID={id}) has been correctly modified");
                return AcceptedAtAction(nameof(UpdateClient),new {id=client.id}, client);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateClient)}){e1.ToString()}");
                return BadRequest("Client object is null");
            } catch (KeyNotFoundException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateClient)}){e2.ToString()}");
                return NotFound("Client object not found");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateClient)}){e3.ToString()}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{key}/accounts/update/{id}")]
        public ActionResult<BankAccount> UpdateAccount(int key, string id, [FromBody] BankAccount account){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            if (id != account.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAccount)}) Account ID({id}) doesnt match body ID({account.id})");
                return BadRequest($"Account ID({id}) doesnt match body ID({account.id})");
            }
            try{
                tecbankService.Account_Update(account);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateAccount)}) Bank account(ID={id}) has been correctly modified");
                return AcceptedAtAction(nameof(UpdateAccount),new {id=account.id}, account);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAccount)}){e1.ToString()}");
                return BadRequest("Account object is null");
            } catch (KeyNotFoundException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAccount)}){e2.ToString()}");
                return NotFound("Account object not found");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAccount)}){e3.ToString()}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{key}/loans/update/{id}")]
        public ActionResult<BankLoan> UpdateLoan(int key, int id, [FromBody] BankLoan loan){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            if (id != loan.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateLoan)}) Loan ID({id}) doesnt match body ID({loan.id})");
                return BadRequest($"Loan ID({id}) doesnt match body ID({loan.id})");
            }
            try{
                tecbankService.Loan_Update(loan);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateLoan)}) Loan(ID={id}) has been correctly modified");
                return AcceptedAtAction(nameof(UpdateLoan),new {id=loan.id}, loan);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateLoan)}){e1.ToString()}");
                return BadRequest("Account object is null");
            } catch (KeyNotFoundException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateLoan)}){e2.ToString()}");
                return NotFound("Account object not found");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateLoan)}){e3.ToString()}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{key}/employees/update/{id}")]
        public ActionResult<BankLoan> UpdateEmployee(int key, int id, [FromBody] BankEmployee employee){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            if (id != employee.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateEmployee)}) Employee ID({id}) doesnt match body ID({employee.id})");
                return BadRequest($"Employee ID({id}) doesnt match body ID({employee.id})");
            }
            try{
                tecbankService.Employee_Update(employee);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateEmployee)}) Employee(ID={id}) has been correctly modified");
                return AcceptedAtAction(nameof(UpdateEmployee),new {id=employee.id}, employee);
            } catch (ArgumentNullException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateEmployee)}){e1.ToString()}");
                return BadRequest("Account object is null");
            } catch (KeyNotFoundException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateEmployee)}){e2.ToString()}");
                return NotFound("Account object not found");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateEmployee)}){e3.ToString()}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{key}/roles/update/{id}")]
        public ActionResult<Role> UpdateRole(int key, int id, [FromBody] Role role)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            if (id != role.id)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateRole)}) Role ID({id}) doesn't match body ID({role.id})");
                return BadRequest($"Role ID({id}) doesn't match body ID({role.id})");
            }

            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(role.name))
                {
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateRole)}) Role name cannot be empty");
                    return BadRequest("Role name cannot be empty");
                }

                tecbankService.UpdateRole(role);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateRole)}) Role(ID={role.id}) was updated successfully");
                return Ok(role);
            }
            catch (ServiceException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateRole)}){e1.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            catch (ArgumentNullException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateRole)}){e2.ToString()}");
                return BadRequest("Role object doesn't have a valid format");
            }
            catch (ArgumentException e3)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateRole)}){e3.ToString()}");
                return BadRequest(e3.Message);
            }
        }

        [HttpPut("{key}/goals/update/{adviserId}/{currencyId}")]
        public ActionResult<AdviserGoal> UpdateAdviserGoal(int key, int adviserId, int currencyId, [FromBody] AdviserGoal goal)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            if (goal.adviser_id != adviserId || goal.currency_id != currencyId)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAdviserGoal)}) Path IDs don't match body IDs");
                return BadRequest("Path IDs don't match body IDs");
            }

            try
            {
                tecbankService.UpdateAdviserGoal(goal);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateAdviserGoal)}) Goal for adviser(ID={adviserId}) with currency(ID={currencyId}) updated successfully");
                return Ok(goal);
            }
            catch (KeyNotFoundException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAdviserGoal)}){e1.ToString()}");
                return NotFound("Goal not found");
            }
            catch (ArgumentNullException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAdviserGoal)}){e2.ToString()}");
                return BadRequest("Goal object is null");
            }
            catch (ServiceException e3)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAdviserGoal)}){e3.ToString()}");
                return StatusCode(500, "Internal server error");
            }
        }

        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
        [HttpDelete("{key}/clients/delete/{id}")]
        public ActionResult DeleteClient(int key, int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                tecbankService.Client_Delete(id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteClient)}) Removed client(ID={id}) successfully from database");
                return Ok($"Client(ID={id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteClient)}){e1.ToString()}");
                return NotFound($"Client(ID={id}) doesn't exist in the database");
            } catch (InvalidOperationException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteClient)}){e2.ToString()}");
                return BadRequest($"Client(ID={id}) has debts to pay");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteClient)}){e3.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("accounts/delete/{id}")]
        public ActionResult DeleteAccount(int key, string id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                tecbankService.Account_Delete(id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteAccount)}) Removed bank account(ID={id}) successfully from database");
                return Ok($"Bank account(ID={id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAccount)}){e1.ToString()}");
                return NotFound($"Bank account(ID={id}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAccount)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("{key}/cards/delete/{num}")]
        public ActionResult DeleteCard(int key,int num){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                tecbankService.Card_Delete(num);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteCard)}) Removed bank card(ID={num}) successfully from database");
                return Ok($"Bank card(ID={num}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteCard)}){e1.ToString()}");
                return NotFound($"Bank card(ID={num}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteCard)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("{key}/employees/delete/{id}")]
        public ActionResult DeleteEmployee(int key,int id){
            if (key != password)
                return StatusCode(500,"Access key for administrator access is invalid");
            try{
                tecbankService.Employee_Delete(id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteEmployee)}) Removed employee(ID={id}) successfully from database");
                return Ok($"Employee(ID={id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteEmployee)}){e1.ToString()}");
                return NotFound($"Employee(ID={id}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteEmployee)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("{key}/roles/delete/{id}")]
        public ActionResult DeleteRole(int key, int id)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            try
            {
                // First check if any employees are using this role
                var employeesWithRole = tecbankService.GetAllEmployees().Where(e => e.role_id == id);
                if (employeesWithRole.Any())
                {
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(DELETE={nameof(DeleteRole)}) Cannot delete role(ID={id}) as it's assigned to employees");
                    return BadRequest($"Cannot delete role as it's currently assigned to {employeesWithRole.Count()} employees");
                }

                tecbankService.RemoveRole(id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteRole)}) Role(ID={id}) was deleted successfully");
                return Ok($"Role(ID={id}) was deleted successfully");
            }
            catch (ServiceException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteRole)}){e1.ToString()}");
                return StatusCode(500, "Internal server error");
            }
            catch (KeyNotFoundException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteRole)}){e2.ToString()}");
                return NotFound($"Role(ID={id}) not found");
            }
        }

        [HttpDelete("{key}/goals/delete/{adviserId}/{currencyId}")]
        public ActionResult DeleteAdviserGoal(int key, int adviserId, int currencyId)
        {
            if (key != password)
                return StatusCode(500, "Access key for administrator access is invalid");
            
            try
            {
                tecbankService.RemoveAdviserGoal(adviserId, currencyId);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(DeleteAdviserGoal)}) Goal for adviser(ID={adviserId}) with currency(ID={currencyId}) deleted successfully");
                return Ok($"Goal for adviser(ID={adviserId}) with currency(ID={currencyId}) deleted successfully");
            }
            catch (KeyNotFoundException e1)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAdviserGoal)}){e1.ToString()}");
                return NotFound("Goal not found");
            }
            catch (ServiceException e2)
            {
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAdviserGoal)}){e2.ToString()}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}