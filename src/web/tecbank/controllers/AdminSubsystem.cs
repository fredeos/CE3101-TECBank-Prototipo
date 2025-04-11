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

        // Listo con XML
        [HttpGet("clients/all")]
        public ActionResult<IEnumerable<ClientAccount>> GetClients(){
            return Ok(tecbankService.GetAllClients());
        }

        // Listo con XML
        [HttpGet("accounts/all")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(){
            return Ok(tecbankService.GetAllAccounts());
        }

        // Listo con XML
        [HttpGet("cards/all")]
        public ActionResult<IEnumerable<BankCard>> GetCards(){
            return Ok(tecbankService.GetAllCards());
        }

        // Listo con XML
        [HttpGet("employees/all")]
        public ActionResult<IEnumerable<BankEmployee>> GetAllEmployees(){
            return Ok(tecbankService.GetAllEmployes());
        }

        // Listo con XML
        [HttpGet("movements/all")]
        public ActionResult<IEnumerable<BankMovement>> GetAllMovements(){
            return Ok(tecbankService.GetAllMovements());
        }

        // Listo con XML
        [HttpGet("loans/all")]
        public ActionResult<IEnumerable<BankLoan>> GetAllLoans(){
            return Ok(tecbankService.GetAllLoans());
        }
        
        // Listo con XML
        [HttpGet("loans/payments/all")]
        public ActionResult<IEnumerable<LoanPayment>> GetAllLoanPayments(){
            return Ok(tecbankService.GetAllPayments());
        }

    
        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------
        // Listo con XML
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


        // Listo con XML
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


        // Listo con XML
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


        // Listo con XML
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


        // ------------------------------------------------- [ POST ] -------------------------------------------------

        // Listo con XML
        [HttpPost("clients/add")]
        public ActionResult<ClientAccount> AddClient([FromBody] ClientAccount client)
        {
            if (client == null)
            {
                return BadRequest("Datos del cliente inválidos.");
            }

            try
            {
                // Validación adicional si es necesaria
                if (client.id <= 0)
                {
                    return BadRequest("El ID del cliente debe ser un número positivo.");
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
                return StatusCode(500, $"Error interno al agregar cliente: {ex.Message}");
            }
        }

        // Listo con XML
        [HttpPost("accounts/add")]
        public ActionResult<BankAccount> AddAccount([FromBody] BankAccount account)
        {
            try
            {
                if (account == null)
                    return BadRequest("Datos de la cuenta inválidos");

                // Validación básica de campos requeridos
                if (account.client_id <= 0 || account.currency_id <= 0)
                    return BadRequest("ID de cliente y moneda son requeridos");

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
                return StatusCode(500, $"Error interno al crear cuenta: {ex.Message}");
            }
        }

        // Listo con XML
        [HttpPost("cards/add")]
        public ActionResult<BankCard> AddCard([FromBody] BankCard card)
        {
            try
            {
                if (card == null)
                    return BadRequest("Datos de la tarjeta inválidos");

                // Validar que se proporcione account_id
                if (string.IsNullOrEmpty(card.account_id))
                    return BadRequest("El número de cuenta es requerido");

                // Generar valores automáticos si no están establecidos
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
                return StatusCode(500, $"Error interno al crear tarjeta: {ex.Message}");
            }
        }

        // Listo con XML
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