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
        [HttpGet("{user}/{pass}")]
        public ActionResult<ClientAccount> Login(String user,String pass){
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
            } catch (System.Exception e1){
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
            var loans = tecbankService.LoansFromClient(user_id);
            if (loans.Count==0){
                return NotFound();
            }
            return Ok(loans);
        }
        // ------------------------------------------------- [ POST ] -------------------------------------------------

        [HttpPost("{user_id}/movements/new")]
        public ActionResult makeMovement(int user_id, [FromBody] BankMovement movement){
            if (movement.account_id == null){
                return BadRequest("El movimiento debe pertenecer a una cuenta");
            }
            return Ok();
        }
        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
    }
}