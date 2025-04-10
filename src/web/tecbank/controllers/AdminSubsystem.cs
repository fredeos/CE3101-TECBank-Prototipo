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
        [HttpGet]
        public ActionResult<IEnumerable<ClientAccount>> Get(){
            return Ok(tecbankService.GetAllEmployes());
        }

        [HttpGet("clients/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetClients(){
            return Ok(tecbankService.GetAllClients());
        }

        [HttpGet("accounts/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetAccounts(){
            return Ok(tecbankService.GetAllAccounts());
        }

        [HttpGet("cards/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetCards(){
            return Ok(tecbankService.GetAllCards());
        }

        [HttpGet("employees/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetAllEmployees(){
            return Ok(tecbankService.GetAllEmployes());
        }
        [HttpGet("movements/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetAllMovements(){
            return Ok(tecbankService.GetAllMovements());
        }

        [HttpGet("loans/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetAllLoans(){
            return Ok(tecbankService.GetAllLoans());
        }
        [HttpGet("loans/payments/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetAllLoanPayments(){
            return Ok(tecbankService.GetAllPayments());
        }

        [HttpGet("loans/{id}")]
        public ActionResult<BankLoan> GetLoan(int id)
        {
            var loan = tecbankService.GetAllLoans().FirstOrDefault(l => l.id == id);
            if (loan == null)
                return NotFound();
            return Ok(loan);
        }
        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------
        [HttpGet("clients/{id}")]
        public ActionResult<ClientAccount> GetClient(int id){
            try{
                var client = tecbankService.Client_findByID(id);
                if (client == null){
                    logService.Log_New(LogTypes.INFO, $"(HTTP)(GET) No matching data was found in the database for client(ID={id})");
                    return NotFound();
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET) Client(ID={id}) was found successfully");
                return Ok(client);
            } catch (System.Exception e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET){e1.ToString()}");
                return BadRequest();
            }
            
        }

        [HttpGet("accounts/{id}")]
        public ActionResult<BankAccount> GetAccount(string id)
        {
            var account = tecbankService.GetAllAccounts().FirstOrDefault(a => a.id == id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpGet("cards/{id}")]
        public ActionResult<BankCard> GetCard(int id)
        {
            var card = tecbankService.GetAllCards().FirstOrDefault(c => c.card_num == id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }
        // ------------------------------------------------- [ POST ] -------------------------------------------------
        [HttpPost("clients/add")]
        public ActionResult AddClient([FromBody] ClientAccount client){
            if (client == null){
                return BadRequest("Datos del cliente inválidos.");
            }
            try{
                tecbankService.Client_Add(client);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST) Client(ID={client.id}) was added successfully");
                return Ok();
            } catch (System.Exception e){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST){e.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpPost("accounts/add")]
        public ActionResult<BankAccount> AddAccount([FromBody] BankAccount account){
            if (account == null)
            {
                return BadRequest("Datos de la cuenta inválidos.");
            }

            tecbankService.Account_Add(account);
            return CreatedAtAction(nameof(GetAccount), new { id = account.id }, account);
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
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST) Client ID({id}) doesnt match body ID({client.id})");
                return BadRequest($"Client ID({id}) doesnt match body ID({client.id})");
            }

            var existingClient = tecbankService.Client_findByID(id);
            if (existingClient == null){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST) Client ID({id}) doesnt exist in the database");
                return NotFound();
            }

            try{
                tecbankService.Client_Update(client);
                return Ok();
            } catch (System.Exception e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST){e1}");
                return StatusCode(500,"Something went wrong");
            }
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