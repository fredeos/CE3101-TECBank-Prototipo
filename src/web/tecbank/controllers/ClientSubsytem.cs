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

        [HttpGet("{user_id}/accounts")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(int user_id){
            var accounts = tecbankService.AccountsFromClient(user_id);
            if (accounts.Count==0){
                return NotFound();
            }
            return Ok(accounts);
        }

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

        [HttpGet("{user_id}/cards")]
        public ActionResult<IEnumerable<BankAccount>> GetCardsPerAccount(int user_id){
            var cards = tecbankService.CardsFromClient(user_id);
            if (cards.Count==0){
                return NotFound();
            }
            return Ok(cards);
        }

        [HttpGet("{user_id}/loans")]
        public ActionResult<IEnumerable<BankAccount>> GetLoans(int user_id){
            var loans = tecbankService.Loans_FromClient(user_id);
            if (loans.Count==0){
                return NotFound();
            }
            return Ok(loans);
        }

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

        [HttpGet("{user_id}/loans/payments")]
        public ActionResult<IEnumerable<LoanPayment>> GetPayments(int user_id){
            var payments = tecbankService.Payments_FromClient(user_id);
            if (payments.Count == 0){
                return NotFound();
            }
            return Ok(payments);
        }

        // ------------------------------------------------- [ POST ] -------------------------------------------------

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