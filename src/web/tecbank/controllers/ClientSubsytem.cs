using Microsoft.AspNetCore.Mvc;
using tecbank.models;
using tecbank.services;

namespace tecbank.controllers{
    [Route("services/client")]
    [ApiController]
    public class Client : ControllerBase {
        private readonly TECBankService tecbankService;
        public Client(TECBankService service){
            this.tecbankService = service;
        }

        [HttpGet]
        public ActionResult<ClientAccount> Home(){
            return NotFound();
        }
        // ------------------------------------------------- [ GET ] -------------------------------------------------
        [HttpGet("login")]
        public ActionResult<ClientAccount> Login([FromBody] String user,[FromForm] String pass){
            var client = tecbankService.Client_find(user, pass);
            if (client == null){
                return NotFound();
            }
            return Ok(client);
        }


        /// <summary>
        /// Retrieves all accounts for a specific client
        /// </summary>
        /// <param name="user_id">Client ID</param>
        /// <returns>List of client accounts</returns>
        /// <response code="200">Returns account list</response>
        /// <response code="404">No accounts found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{user_id}/accounts")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(int user_id)
        {
            try
            {
                // Get accounts using the service layer
                var accounts = tecbankService.AccountsFromClient(user_id);
                
                // Return 404 if no accounts found
                if (accounts.Count == 0)
                {
                    return NotFound();
                }
                
                // Return 200 with accounts data
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                // Log error and return 500 for unexpected errors
                return StatusCode(500, $"Error retrieving accounts: {ex.Message}");
            }
        }

        // Implementado por Frederick
        [HttpGet("{user_id}/{account_id}/cards")]
        public ActionResult<IEnumerable<BankAccount>> GetCardsPerAccount(int user_id, String account_id){
            try {
                var cards = tecbankService.CardsFromAccount(user_id, account_id);
                if (cards.Count == 0){
                    return NotFound();
                }
                return Ok(cards);
            } catch {
                return BadRequest("Datos del cliente o cuenta bancaria incorrectos");
            }
        }


       /// <summary>
        /// Retrieves all cards associated with a client's accounts
        /// </summary>
        /// <param name="user_id">Client ID</param>
        /// <returns>List of bank cards</returns>
        /// <response code="200">Returns card list</response>
        /// <response code="404">No cards found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{user_id}/cards")]
        public ActionResult<IEnumerable<BankCard>> GetCardsPerAccount(int user_id)
        {
            try
            {
                // Retrieve all cards linked to client's accounts
                var cards = tecbankService.CardsFromClient(user_id);
                
                // Return 404 if no cards found
                if (!cards.Any())
                {
                    return NotFound($"No cards found for user {user_id}");
                }
                
                // Return 200 with cards data
                return Ok(cards);
            }
            catch (Exception ex)
            {
                // Log error and return 500 for unexpected errors
                return StatusCode(500, $"Error retrieving cards: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all loans for a specific client
        /// </summary>
        /// <param name="user_id">Client identifier</param>
        /// <returns>List of client loans</returns>
        /// <response code="200">Returns loan list</response>
        /// <response code="404">No loans found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{user_id}/loans")]
        public ActionResult<IEnumerable<BankLoan>> GetLoans(int user_id)
        {
            try
            {
                // Retrieve all loans belonging to the specified client
                var loans = tecbankService.LoansFromClient(user_id);
                
                // Return 404 if no loans found
                if (!loans.Any())
                {
                    return NotFound($"No loans found for client {user_id}");
                }
                
                // Return 200 with loans data
                return Ok(loans);
            }
            catch (Exception ex)
            {
                // Log error and return 500 for unexpected errors
                return StatusCode(500, $"Error retrieving loans: {ex.Message}");
            }
        }

        // Implementación de Frederick
        [HttpGet("{user_id}/{acc_id}/movements")]
        public ActionResult<IEnumerable<BankMovement>> GetMovements(int user_id, String acc_id){
            try{
                var movements = tecbankService.Movements_FromAccount(acc_id,user_id);
                if (movements.Count == 0){
                    return NotFound();
                }
                return Ok(movements);
            } catch (System.Exception e1){
                return BadRequest(e1);
            }
        }

        /// <summary>
        /// Retrieves all loan payments for a specific client
        /// </summary>
        /// <param name="user_id">Client identifier</param>
        /// <returns>List of loan payments</returns>
        /// <response code="200">Returns payment list</response>
        /// <response code="404">No payments found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{user_id}/loans/payments")]
        public ActionResult<IEnumerable<LoanPayment>> GetPayments(int user_id)
        {
            try
            {
                var payments = tecbankService.GetClientLoanPayments(user_id);
                
                if (!payments.Any())
                {
                    return NotFound($"No payments found for client {user_id}");
                }
                
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving loan payments: {ex.Message}");
            }
        }


        // ------------------------------------------------- [ POST ] -------------------------------------------------
        
        // Falta la implementación de todos los post

        [HttpPost("{user_id}/movements/new")]
        public ActionResult makeMovement(int user_id, [FromBody] BankMovement movement){
            if (movement.account_id == null){
                return BadRequest("El movimiento debe pertenecer a una cuenta");
            }
            try{
                tecbankService.Movement_New(user_id, movement);
            } catch (System.Exception e1){
                return BadRequest(e1);
            }
            return Ok();
        }

        [HttpPost("{user_id}/{card_num}/movements/new")]
        public ActionResult makeMovementWithCard(int user_id, int card_num){
            return Ok();
        }

        [HttpPost("{user_id}/loans/payment/{account_id}")]
        public ActionResult makePayment(int user_id, String account_id,[FromBody] LoanPayment payment){
            tecbankService.Payment_MakeAPayment(user_id, account_id, payment);
            return Ok();
        }
        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
    }
}