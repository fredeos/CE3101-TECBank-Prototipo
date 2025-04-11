using Microsoft.AspNetCore.Mvc;
using tecbank.models;
using tecbank.services;
using tecbank.services.logger;

namespace tecbank.controllers{
    [Route("services/client")]
    [ApiController]
    public class Client : ControllerBase {
        private readonly TECBankService tecbankService;
        private readonly LogService logService;
        public Client(TECBankService service, LogService log){
            this.tecbankService = service;
            this.logService = log;
        }

        // ------------------------------------------------- [ GET ] -------------------------------------------------
        [HttpGet("login")]
        public ActionResult<ClientAccount> Login(String user,String pass){
            try{
                var client = tecbankService.Client_find(user, pass);
                if (client == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(Login)}) Client login failed, no such client exists");
                    return NotFound("Client login failed, no such client exists");
                }
                return Ok(client);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(Login)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpGet("{user_id}/accounts")]
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(int user_id){
            try{
                var accounts = tecbankService.Accounts_FromClient(user_id);
                if (accounts.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAccounts)}) No banck accounts are bound to client(ID={user_id})");
                    return NotFound();
                }
                return Ok(accounts); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccounts)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("{user_id}/{account_id}/cards")]
        public ActionResult<IEnumerable<BankAccount>> GetCardsPerAccount(int user_id, String account_id){
            try {
                var cards = tecbankService.Cards_FromAccount(user_id, account_id);
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
            var cards = tecbankService.Cards_FromClient(user_id);
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
        [HttpPost("login/new")]
        public ActionResult<ClientAccount> Register([FromBody] ClientAccount client){
            if (client == null){
                return BadRequest("Datos del cliente inválidos.");
            }
            try{
                tecbankService.Client_Add(client);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(Register)}) Client(ID={client.id}) was added successfully");
                return Ok();
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e2.ToString()}");
                return BadRequest("Client object doesn't have valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e3.ToString()}");
                return BadRequest("Failed to add client to database");
            }
        }

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