using Microsoft.AspNetCore.Mvc;
using tecbank.models;
using tecbank.services;

namespace tecbank.controllers{
    [Route("services/admin")]
    [ApiController]
    public class Admin : ControllerBase {
        private readonly TECBankService tecbankService;
        public Admin(TECBankService service){
            this.tecbankService = service;
        }

        // ------------------------------------------------- [ General GET ] -------------------------------------------------

        /// <summary>
        /// Retrieves all client accounts from the system
        /// </summary>
        /// <returns>List of all client accounts</returns>
        /// <response code="200">Returns the complete client list</response>
        [HttpGet("clients/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetClients(){
            return Ok(tecbankService.GetAllClients());
        }

        /// <summary>
        /// Retrieves all bank accounts from the system
        /// </summary>
        /// <returns>List of all bank accounts</returns>
        /// <response code="200">Returns complete account list</response>
        [HttpGet("accounts/all")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(){
            return Ok(tecbankService.GetAllAccounts());
        }

        /// <summary>
        /// Retrieves all bank cards from the system
        /// </summary>
        /// <returns>List of all bank cards</returns>
        /// <response code="200">Success - Returns card list</response>
        [HttpGet("cards/all")]
        public ActionResult<IEnumerable<BankCard>> GetCards(){
            return Ok(tecbankService.GetAllCards());
        }

        /// <summary>
        /// Retrieves all bank employees
        /// </summary>
        /// <returns>List of all employees</returns>
        /// <response code="200">Returns complete employee list</response>
        [HttpGet("employees/all")]
        public ActionResult<IEnumerable<BankEmployee>> GetAllEmployees(){
            return Ok(tecbankService.GetAllEmployes());
        }

        /// <summary>
        /// Retrieves all loan advisers (role_id = 2)
        /// </summary>
        /// <returns>List of loan advisers</returns>
        /// <response code="200">Returns loan adviser list</response>
        /// <response code="404">No loan advisers found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("advisers/all")]
        public ActionResult<IEnumerable<BankEmployee>> GetAllLoanAdvisers()
        {
            try
            {
                // Get all employees with role_id = 2 (loan advisers)
                var advisers = tecbankService.GetAllLoanAdvisers();
                
                if (!advisers.Any())
                {
                    return NotFound("No loan advisers found");
                }
                
                return Ok(advisers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving loan advisers: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all bank movements
        /// </summary>
        /// <returns>List of all transactions</returns>
        /// <response code="200">Returns complete movement list</response>
        [HttpGet("movements/all")]
        public ActionResult<IEnumerable<BankMovement>> GetAllMovements(){
            return Ok(tecbankService.GetAllMovements());
        }

        /// <summary>
        /// Retrieves all loan records
        /// </summary>
        /// <returns>List of all loans</returns>
        /// <response code="200">Returns complete loan list</response>
        [HttpGet("loans/all")]
        public ActionResult<IEnumerable<BankLoan>> GetAllLoans(){
            return Ok(tecbankService.GetAllLoans());
        }
        
        /// <summary>
        /// Retrieves all loan payments
        /// </summary>
        /// <returns>List of all payments</returns>
        /// <response code="200">Returns complete payment list</response>
        [HttpGet("loans/payments/all")]
        public ActionResult<IEnumerable<LoanPayment>> GetAllLoanPayments(){
            return Ok(tecbankService.GetAllPayments());
        }

    
        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------

        /// <summary>
        /// Retrieves a client by ID
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>Client account details</returns>
        /// <response code="200">Client found</response>
        /// <response code="404">Client not found</response>
        [HttpGet("clients/{id}")]
        public ActionResult<ClientAccount> GetClient(int id){
            try{
                var client = tecbankService.Client_findByID(id);
                if (client == null){
                    return NotFound();
                }
                return Ok(client);
            } catch (System.Exception e1){
                Console.WriteLine(e1);
                return NotFound();
            }
            
        }


        /// <summary>
        /// Retrieves a loan by ID
        /// </summary>
        /// <param name="id">Loan ID</param>
        /// <returns>Loan details</returns>
        /// <response code="200">Loan found</response>
        /// <response code="404">Loan not found</response>
        /// <response code="500">Database error</response>
        [HttpGet("loans/{id}")]
        public ActionResult<BankLoan> GetLoan(int id)
        {
            try
            {
                var loan = tecbankService.GetLoanById(id);
                return Ok(loan);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves a bank account by ID
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Account details</returns>
        /// <response code="200">Account found</response>
        /// <response code="404">Account not found</response>
        /// <response code="500">Database error</response>
        [HttpGet("accounts/{id}")]
        public ActionResult<BankAccount> GetAccount(string id)
        {
            try
            {
                var account = tecbankService.GetAccountById(id);
                return Ok(account);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Retrieves a bank card by card number
        /// </summary>
        /// <param name="id">Card number</param>
        /// <returns>Card details</returns>
        /// <response code="200">Card found</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Database error</response>
        [HttpGet("cards/{id}")]
        public ActionResult<BankCard> GetCard(int id)
        {
            try
            {
                var card = tecbankService.GetCardByNumber(id);
                return Ok(card);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a loan adviser by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Loan adviser details</returns>
        /// <response code="200">Adviser found</response>
        /// <response code="404">Adviser not found or not a loan adviser</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("advisers/{id}")]
        public ActionResult<BankEmployee> GetLoanAdviserById(int id)
        {
            try
            {
                // Get specific adviser by ID (must have role_id = 2)
                var adviser = tecbankService.GetLoanAdviserById(id);
                
                if (adviser == null)
                {
                    return NotFound($"Loan adviser with ID {id} not found");
                }
                
                return Ok(adviser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving loan adviser: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        /// <response code="200">Employee found</response>
        /// <response code="404">Employee not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("employees/{id}")]
        public ActionResult<BankEmployee> GetEmployeeById(int id)
        {
            try
            {
                var employee = tecbankService.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound($"No se encontró un empleado con ID {id}");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener empleado: {ex.Message}");
            }
        }


        // ------------------------------------------------- [ POST ] -------------------------------------------------

        /// <summary>
        /// Creates a new employee record
        /// </summary>
        /// <param name="employee">Employee data</param>
        /// <returns>Created employee</returns>
        /// <response code="201">Employee successfully created</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="409">Employee already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("employees/add")]
        public ActionResult<BankEmployee> AddEmployee([FromBody] BankEmployee employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data");
            }

            try
            {
                // Validación básica
                if (employee.id <= 0)
                {
                    return BadRequest("Employee ID must be positive");
                }

                if (string.IsNullOrWhiteSpace(employee.name))
                {
                    return BadRequest("Employee name is required");
                }

                tecbankService.CreateEmployee(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.id }, employee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while creating employee: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new client to the system
        /// </summary>
        /// <param name="client">Client data to add</param>
        /// <returns>ActionResult with created client</returns>
        /// <response code="201">Returns the newly created client</response>
        /// <response code="400">If client data is invalid</response>
        /// <response code="409">If client already exists</response>
        /// <response code="500">If internal server error occurs</response>
        [HttpPost("clients/add")]
        public ActionResult<ClientAccount> AddClient([FromBody] ClientAccount client)
        {
            if (client == null)
            {
                return BadRequest("Invalid client data");
            }

            try
            {
                // Validación adicional si es necesaria
                if (client.id <= 0)
                {
                    return BadRequest("Client ID must be a positive number");
                }

                tecbankService.Client_Add(client);
                return CreatedAtAction(nameof(GetClient), new { id = client.id }, client);
            }
            catch (InvalidOperationException ex)
            {
                // Cliente ya existe
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                // Otros errores
                return StatusCode(500, $"Internal server error while adding client: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new bank account to the system
        /// </summary>
        /// <param name="account">Account data to add</param>
        /// <returns>ActionResult with created account</returns>
        /// <response code="201">Returns the newly created account</response>
        /// <response code="400">If account data is invalid</response>
        /// <response code="500">If internal server error occurs</response>
        [HttpPost("accounts/add")]
        public ActionResult<BankAccount> AddAccount([FromBody] BankAccount account)
        {
            try
            {
                if (account == null)
                    return BadRequest("Invalid account data");

                // Validación básica de campos requeridos
                if (account.client_id <= 0 || account.currency_id <= 0)
                    return BadRequest("Client ID and currency ID are required");

                // Generar ID costarricense si no viene especificado
                if (string.IsNullOrEmpty(account.id))
                {
                    account.id = tecbankService.GenerateCostaRicanAccountId();
                }

                tecbankService.Account_Add(account);
                return CreatedAtAction(nameof(GetAccount), new { id = account.id }, account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while creating account: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new bank card to the system
        /// </summary>
        /// <param name="card">Card data to add</param>
        /// <returns>ActionResult with created card</returns>
        /// <response code="201">Returns the newly created card</response>
        /// <response code="400">If card data is invalid</response>
        /// <response code="500">If internal server error occurs</response>
        [HttpPost("cards/add")]
        public ActionResult<BankCard> AddCard([FromBody] BankCard card)
        {
            try
            {
                // Null check for card object
                if (card == null)
                    return BadRequest("Invalid card data");

                // Validate that account_id is provided
                if (string.IsNullOrEmpty(card.account_id))
                    return BadRequest("Account number is required");

                // Generate automatic values ​​if not set
                if (card.card_num == 0)
                    card.card_num = tecbankService.GenerateCardNumber();
                
                if (card.cvc == 0)
                    card.cvc = tecbankService.GenerateCVC();

                tecbankService.Card_Add(card);
                return CreatedAtAction(nameof(GetCard), new { id = card.card_num }, card);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error creating card: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new loan in the system
        /// </summary>
        /// <param name="loan">Loan data to create</param>
        /// <returns>Created loan information</returns>
        /// <response code="201">Loan successfully created</response>
        /// <response code="400">Invalid loan data</response>
        /// <response code="500">Database operation error</response>
        [HttpPost("loans/add")]
        public ActionResult<BankLoan> AddLoan([FromBody] BankLoan loan)
        {
            try
            {
                if (loan == null)
                    return BadRequest("Invalid loan data");

                tecbankService.Loan_Add(loan);
                return CreatedAtAction(nameof(GetLoan), new { id = loan.id }, loan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SystemException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        
        [HttpPut("clients/update/{id}")]
        public ActionResult UpdateClient(int id, [FromBody] ClientAccount client)
        {
            if (id != client.id)
            {
                return BadRequest("ID del cliente no coincide.");
            }

            var existingClient = tecbankService.Client_findByID(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            tecbankService.Client_Update(client);
            return NoContent();
        }

        [HttpPut("accounts/update/{id}")]
        public ActionResult UpdateAccount(string id, [FromBody] BankAccount account)
        {
            if (id != account.id)
            {
                return BadRequest("ID de la cuenta no coincide.");
            }

            var existingAccount = tecbankService.GetAllAccounts().FirstOrDefault(a => a.id == id);
            if (existingAccount == null)
            {
                return NotFound();
            }

            tecbankService.Account_Update(account);
            return NoContent();
        }

        [HttpPut("loans/update/{id}")]
        public ActionResult UpdateLoan(int id, [FromBody] BankLoan loan)
        {
            try
            {
                if (id != loan.id)
                    return BadRequest("ID del préstamo no coincide.");

                tecbankService.Loan_Update(loan);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
        [HttpDelete("clients/delete/{id}")]
        public ActionResult DeleteClient(int id)
        {
            var client = tecbankService.Client_findByID(id);
            if (client == null)
            {
                return NotFound();
            }

            tecbankService.Client_Delete(id);
            return NoContent();
        }

        [HttpDelete("accounts/delete/{id}")]
        public ActionResult DeleteAccount(string id)
        {
            var account = tecbankService.GetAllAccounts().FirstOrDefault(a => a.id == id);
            if (account == null)
            {
                return NotFound();
            }

            tecbankService.Account_Delete(id);
            return NoContent();
        }

        [HttpDelete("cards/delete/{id}")]
        public ActionResult DeleteCard(int id)
        {
            var card = tecbankService.GetAllCards().FirstOrDefault(c => c.card_num == id);
            if (card == null)
            {
                return NotFound();
            }

            tecbankService.Card_Delete(id);
            return NoContent();
        }
    }
}