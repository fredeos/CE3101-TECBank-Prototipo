using Microsoft.AspNetCore.Http.HttpResults;
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
        public Admin(TECBankService service, LogService log){
            this.tecbankService = service;
            this.logService = log;
        }

        // ------------------------------------------------- [ General GET ] -------------------------------------------------
        [HttpGet("clients/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetClients(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetClients)}) Clients retrieved succesfully");
                return Ok(tecbankService.GetAllClients());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetClients)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("accounts/all")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAccounts)}) Bank accounts retrieved succesfully");
                return Ok(tecbankService.GetAllAccounts());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccounts)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("cards/all")]
        public ActionResult<IEnumerable<BankCard>> GetCards(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCards)}) Bank cards retrieved succesfully");
                return Ok(tecbankService.GetAllCards());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCards)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("employees/all")]
        public ActionResult<IEnumerable<Employee>> GetEmployees(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetEmployees)}) Employees retrieved succesfully");
                return Ok(tecbankService.GetAllEmployees());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetEmployees)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }
        [HttpGet("movements/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetMovements(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetMovements)}) Bank movements retrieved succesfully");
                return Ok(tecbankService.GetAllMovements());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetMovements)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("loans/all")]
        public ActionResult<IEnumerable<BankLoan>> GetLoans(){
           try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetEmployees)}) Loans retrieved succesfully");
                return Ok(tecbankService.GetAllLoans());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetEmployees)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("loans/payments/all")]
        public ActionResult<IEnumerable<LoanPayment>> GetLoanPayments(){
            try{
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoanPayments)}) Loan payments retrieved succesfully");
                return Ok(tecbankService.GetAllPayments());
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoanPayments)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------
        [HttpGet("clients/{id}")]
        public ActionResult<ClientAccount> GetClient(int id){
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

        [HttpGet("accounts/{id}")]
        public ActionResult<BankAccount> GetAccount(string id){
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

        [HttpGet("cards/{id}")]
        public ActionResult<BankCard> GetCard(int id){
            try{
                var card = tecbankService.Card_Get(id);
                if (card == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetCard)}) No matching data was found in the database for card(ID={id})");
                    return NotFound($"(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCard)}) Bank card(ID={id}) was found successfully");
                return Ok(card);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCard)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        [HttpGet("loans/{id}")]
        public ActionResult<BankLoan> GetLoan(int id){
            try{
                var loan = tecbankService.Loan_Get(id);
                if (loan == null){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetLoan)}) No matching data was found in the database for loan(ID={id})");
                    return NotFound($"(ID={id}) not found");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoan)}) Bank loan(ID={id}) was found successfully");
                return Ok(loan);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoan)}){e1.ToString()}");
                return StatusCode(500,"Something went wrong");
            }
        }

        // ------------------------------------------------- [ POST ] -------------------------------------------------
        [HttpPost("clients/add")]
        public ActionResult AddClient([FromBody] ClientAccount client){
            try{
                if (client.id <= 0){
                    logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(AddClient)}) Client.id is negative and therefore can't be added to the database");
                    return BadRequest("Client.id must be positive");
                }

                tecbankService.Client_Add(client);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddClient)}) Client(ID={client.id}) was added successfully");
                return Ok();
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

        [HttpPost("accounts/add")]
        public ActionResult<BankAccount> AddAccount([FromBody] BankAccount account){
            try{
                var owner = tecbankService.Client_findByID(account.client_id);
                if (owner == null){
                    logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddAccount)}) Bank account(ID={account.id}) doesn't belong to a known client");
                    return BadRequest("Owner of account doesn't exist in the database");
                }
                tecbankService.Account_Add(account);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(AddAccount)}) Bank account(ID={account.id}) was added successfully");
                return Ok();
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

        [HttpPost("cards/add")]
        public ActionResult<BankCard> AddCard([FromBody] BankCard card)
        {
            if (card == null)
            {
                return BadRequest("Datos de la tarjeta inválidos.");
            }

            tecbankService.Card_Add(card);
            return CreatedAtAction(nameof(GetCard), new { id = card.card_num }, card);
        }

        [HttpPost("loans/add")]
        public ActionResult<BankLoan> AddLoan([FromBody] BankLoan loan){
            try
            {
                if (loan == null)
                    return BadRequest("Datos del préstamo inválidos.");

                // Validar que el monto total sea positivo
                if (loan.total <= 0)
                    return BadRequest("El monto total debe ser positivo.");

                loan.request_date = DateTime.Now; // Fecha automática
                loan.balance = loan.total; // Saldo inicial = monto total

                tecbankService.Loan_Add(loan);
                return CreatedAtAction(nameof(GetLoan), new { id = loan.id }, loan);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        [HttpPut("clients/update/{id}")]
        public ActionResult UpdateClient(int id, [FromBody] ClientAccount client){
            if (id != client.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateClient)}) Client ID({id}) doesnt match body ID({client.id})");
                return BadRequest($"Client ID({id}) doesnt match body ID({client.id})");
            }
            try{
                tecbankService.Client_Update(client);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateClient)}) Client(ID={id}) has been correctly modified");
                return Ok();
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

        [HttpPut("accounts/update/{id}")]
        public ActionResult UpdateAccount(string id, [FromBody] BankAccount account){
            if (id != account.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateAccount)}) Account ID({id}) doesnt match body ID({account.id})");
                return BadRequest($"Account ID({id}) doesnt match body ID({account.id})");
            }
            try{
                tecbankService.Account_Update(account);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateAccount)}) Bank account(ID={id}) has been correctly modified");
                return Ok();
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

        [HttpPut("loans/update/{id}")]
        public ActionResult UpdateLoan(int id, [FromBody] BankLoan loan){
            if (id != loan.id){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(PUT={nameof(UpdateLoan)}) Loan ID({id}) doesnt match body ID({loan.id})");
                return BadRequest($"Loan ID({id}) doesnt match body ID({loan.id})");
            }
            try{
                tecbankService.Loan_Update(loan);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateLoan)}) Loan(ID={id}) has been correctly modified");
                return Ok();
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

        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
        [HttpDelete("clients/delete/{id}")]
        public ActionResult DeleteClient(int id){
            try{
                tecbankService.Client_Delete(id);
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
        public ActionResult DeleteAccount(string id){
            try{
                tecbankService.Account_Delete(id);
                return Ok($"Bank account(ID={id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAccount)}){e1.ToString()}");
                return NotFound($"Bank account(ID={id}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteAccount)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("cards/delete/{num}")]
        public ActionResult DeleteCard(int num){
            try{
                tecbankService.Card_Delete(num);
                return Ok($"Bank card(ID={num}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteCard)}){e1.ToString()}");
                return NotFound($"Bank account(ID={num}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(DeleteCard)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }
    }
}