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
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(Login)}) Client(ID={client.id}) login succesful");
                return Ok(client);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(Login)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            }
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
        public ActionResult<IEnumerable<BankAccount>> GetAccounts(int user_id){
            try{
                var accounts = tecbankService.Accounts_FromClient(user_id);
                if (accounts.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAccounts)}) No bank accounts are bound to client(ID={user_id})");
                    return NotFound($"No bank accounts were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAccounts)}) Bank accounts from client(ID={user_id}) found succesfullly");
                return Ok(accounts); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccounts)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAccounts)}){e2.ToString()}");
                return StatusCode(500, $"Client(ID={user_id}) was not found");
            }
        }

        [HttpGet("{user_id}/{account_id}/cards")]
        public ActionResult<IEnumerable<BankCard>> GetCardsPerAccount(int user_id, String account_id){
            try{
                var cards = tecbankService.Cards_FromAccount(user_id, account_id);
                if (cards.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetCardsPerAccount)}) No credit/debit cards are bound to bank account(ID={account_id}) from client(ID={user_id})");
                    return NotFound($"No credit/debit cards were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCardsPerAccount)}) Credit/debit cards from account(ID={account_id}) found succesfullly");
                return Ok(cards); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerAccount)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerAccount)}){e2.ToString()}");
                return BadRequest($"Client informaton is not valid");
            }
        }

        [HttpGet("{user_id}/cards")]
        public ActionResult<IEnumerable<BankCard>> GetCardsPerClient(int user_id){
            try{
                var cards = tecbankService.Cards_FromClient(user_id);
                if (cards.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetCardsPerClient)}) No credit/debit cards are bound to client(ID={user_id})");
                    return NotFound($"No credit/debit cards were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCardsPerClient)}) Credit/debit cards from client(ID={user_id}) found succesfullly");
                return Ok(cards); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerClient)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerClient)}){e2.ToString()}");
                return BadRequest($"Client informaton not valid");
            }
        }

        [HttpGet("{user_id}/loans")]
        public ActionResult<IEnumerable<BankLoan>> GetLoans(int user_id){
            try{
                var loans = tecbankService.Loans_FromClient(user_id);
                if (loans.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetLoans)}) No loans are bound to client(ID={user_id})");
                    return NotFound($"No loans were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoans)}) Loans from client(ID={user_id}) found succesfullly");
                return Ok(loans);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoans)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoans)}){e2.ToString()}");
                return BadRequest($"Client informaton not valid");
            }
        }

        [HttpGet("{user_id}/{acc_id}/movements")]
        public ActionResult<IEnumerable<BankMovement>> GetMovements(int user_id, String acc_id){
            try{
                var movements = tecbankService.Movements_FromAccount(acc_id,user_id);
                if (movements.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetCardsPerClient)}) No movements cards are bound to bank account(ID={acc_id}) from client(ID={user_id})");
                    return NotFound($"No movements were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetCardsPerClient)}) Movements from client(ID={user_id}) found succesfullly");
                return Ok(movements);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerClient)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetCardsPerClient)}){e2.ToString()}");
                return BadRequest($"Client informaton not valid");
            }
        }

        [HttpGet("{user_id}/loans/payments/all")]
        public ActionResult<IEnumerable<LoanPayment>> GetAllPayments(int user_id){
            try{
                var payments = tecbankService.LoanPayments_FromClient(user_id);
                if (payments.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetAllPayments)}) No loan payments are bound to client(ID={user_id})");
                    return NotFound($"No loans were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetAllPayments)}) Loans from client(ID={user_id}) found succesfullly");
                return Ok(payments); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAllPayments)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetAllPayments)}){e2.ToString()}");
                return BadRequest($"Client informaton not valid");
            }
        }

        [HttpGet("{user_id}/loans/payments/{loan_id}")]
        public ActionResult<IEnumerable<LoanPayment>> GetLoanPayments(int user_id, int loan_id){
            try{
                var payments = tecbankService.LoanPayments_FromLoan(user_id, loan_id);
                if (payments.Count==0){
                    logService.Log_New(LogTypes.WARNING, $"(HTTP)(GET={nameof(GetLoanPayments)}) No loan payments are bound to loan(ID={loan_id}) from client(ID={user_id})");
                    return NotFound($"No loan payments were found for client(ID={user_id})");
                }
                logService.Log_New(LogTypes.INFO, $"(HTTP)(GET={nameof(GetLoanPayments)}) Loans from client(ID={user_id}) found succesfullly");
                return Ok(payments); 
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoanPayments)}){e1.ToString()}");
                return StatusCode(500, "Something went wrong");
            } catch (ArgumentException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(GET={nameof(GetLoanPayments)}){e2.ToString()}");
                return BadRequest($"Client or loan informaton not valid");
            }
        }

        // ------------------------------------------------- [ POST ] -------------------------------------------------
        [HttpPost("login/new")]
        public ActionResult<ClientAccount> Register([FromBody] ClientAccount client){
            try{
                tecbankService.Client_Add(client);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(Register)}) Client(ID={client.id}) was added successfully");
                return CreatedAtAction(nameof(Register),new {id=client.id}, client);
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

        [HttpPost("{user_id}/loans/payment/{account_id}")]
        public ActionResult<LoanPayment> MakeLoanPayment(int user_id, String account_id,[FromBody] LoanPayment payment){
            try{
                tecbankService.Payment_MakeAPayment(user_id, account_id, payment);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(Register)}) Loan payment(ID={payment.id}) has been added to database");
                return CreatedAtAction(nameof(MakeLoanPayment), new {id=payment.id}, payment);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e2.ToString()}");
                return BadRequest("Payment object doesn't have valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e3.ToString()}");
                return BadRequest("Failed to add a new payment to database");
            } catch (InvalidOperationException e4){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(Register)}){e4.ToString()}");
                return BadRequest($"Specified bank account(ID={account_id}) by client can't be used to make a loan payment");
            }
        }

        [HttpPost("{user_id}/accounts/new")]
        public ActionResult<BankAccount> OpenAccount(int user_id, [FromBody] BankAccount account){
            if (user_id != account.client_id){
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(OpenAccount)}) Client(ID={user_id}) is different from bank account client(ID={account.client_id})");
                return BadRequest($"Client.Id({user_id}) is differenct from the Client.Id({account.client_id}) from the bank account");
            }
            try{
                tecbankService.Account_Add(account);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(OpenAccount)}) Bank account(ID={account.id}) has been added for client(ID={user_id}) to database");
                return CreatedAtAction(nameof(OpenAccount), new {id=account.id}, account);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(OpenAccount)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(OpenAccount)}){e2.ToString()}");
                return BadRequest("Bank account doesn't have a valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(OpenAccount)}){e3.ToString()}");
                return BadRequest("Failed to add a new bank account to database");
            }
        }

        [HttpPost("{user_id}/cards/new")]
        public ActionResult<BankCard> CreateCard(int user_id, [FromBody] BankCard card){
            try{
                var card_account = tecbankService.Account_Get(card.account_id);
                if (card_account == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(CreateCard)}) Bank account(ID={card.account_id}) from card doesn't exist on the database");
                    return NotFound("Bank account from card doesn't exist");
                } else {
                    if (card_account.client_id != user_id){
                        logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(CreateCard)}) Bank account(ID={card.account_id}) from card doesn't exist on the database");
                        return BadRequest($"Client(ID={user_id}) isn't the owner of the bank account(ID={card_account.id}) from card");
                    }
                }
                tecbankService.Card_Add(card);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(CreateCard)}) Card(ID={card.card_num}) has been added for client(ID={user_id}) to database");
                return CreatedAtAction(nameof(CreateCard), new {id=card.card_num}, card);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(CreateCard)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (ArgumentNullException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(CreateCard)}){e2.ToString()}");
                return BadRequest("Card doesn't have a valid format");
            } catch (ArgumentException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(CreateCard)}){e3.ToString()}");
                return BadRequest("Failed to add a new card to database");
            }
        }

        [HttpPost("{user_id}/movements/new/{owner_id}/{target_id}")]
        public ActionResult<BankMovement> MakeTransfer(int user_id, String owner_id, String target_id, [FromBody] BankMovement transfer){
            // >> Verificaciones para tipo de transaccion
            if (transfer.total_transfer < 0){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeTransfer)}) Transfers between account can't be negative");
                return BadRequest($"Transfer amount can't be negative");
            }
            try{
                // >> Verificar la existencia de la cuenta del dueño y que esta pertenezca al cliente
                var owner_account = tecbankService.Account_Get(owner_id);
                if (owner_account == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeTransfer)}) Bank account(ID={owner_id}) from card doesn't exist on the database");
                    return NotFound($"Bank account(ID={owner_id}) doesn't exist");
                } else {
                    if (owner_account.client_id != user_id){
                        logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeTransfer)}) Bank account(ID={owner_id}) doesn't belong to client(ID={user_id}) in the database");
                        return BadRequest($"Client(ID={user_id}) isn't the owner of the bank account(ID={owner_id})");
                    }
                }
                // >> Generar el movimiento para ambas cuentas
                tecbankService.Movement_New_AccountToAccount(owner_id,target_id,transfer);
                logService.Log_New(LogTypes.INFO,$"(HTTP)(POST={nameof(MakeTransfer)}) Movement(ID={transfer.id}) has been added to database");
                return CreatedAtAction(nameof(MakeTransfer), new {id=transfer.id}, transfer);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeTransfer)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (InvalidOperationException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeTransfer)}){e2.ToString()}");
                return BadRequest($"Either account(ID={owner_id}) or used credit card(ID={transfer.card_id}) doesn't have enough funds to proceed with transaction");
            } catch (ArgumentNullException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeTransfer)}){e3.ToString()}");
                return BadRequest("Transfer movement doesn't have a valid format");
            } catch (ArgumentException e4){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeTransfer)}){e4.ToString()}");
                return BadRequest("Failed to add a new movement to the database");
            }
        }

        [HttpPost("{user_id}/cards/{card_num}/movement/new")]
        public ActionResult<BankCard> MakeCardTransfer(int user_id, int card_num, [FromBody] BankMovement transfer){
            // >> Verificaciones para la tarjeta
            if (transfer.card_id != card_num){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={MakeCardTransfer}) Card(ID={card_num}) doesn't match the card id({transfer.card_id}) on movement object");
                return BadRequest($"Card(ID={card_num}) doesn't match the card id({transfer.card_id}) on movement object");
            }
            // >> Verificaciones para tipo de transaccion
            if (transfer.type == 2 && transfer.total_transfer < 0){ // Pago de deuda de tarjeta
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={MakeCardTransfer}) Credit debt payment for card(ID={card_num}) can't be negative");
                return BadRequest($"Credit debt payment for card(ID={card_num}) can't be negative");
            }
            if (transfer.type == 4 && transfer.total_transfer > 0){ // Retiro de dinero en ATM
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Withdrawal for card(ID={card_num}) can't be positive");
                return BadRequest($"Withdrawal for card(ID={card_num}) can't be positive");
            }
            if (transfer.type == 5 && transfer.total_transfer < 0){ // Configuracion de limite para tarjetas de debito
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Debit card(ID={card_num}) limit can't be negative");
                return BadRequest($"Expense limit for debit card(ID={card_num}) can't be negative");
            }
            try{
                // >> Verificar existencia del cliente
                var client = tecbankService.Client_findByID(user_id);
                if (client == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Client(ID={user_id}) doesn't exist on database");
                    return NotFound($"Client(ID={user_id}) not found");
                }
                // >> Verificar que el cliente es dueño de la tarjeta
                var card = tecbankService.Card_Get(card_num);
                if(card == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Card(ID={card_num}) doesn't exist on database");
                    return NotFound($"Card(ID={card_num}) not found");
                }
                var card_account = tecbankService.Account_Get(card.account_id);
                if(card_account == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Bank account(ID={card.account_id}) from card(ID={card_num}) doesn't exist on database");
                    return NotFound($"Bank account(ID={card.account_id}) from card(ID={card_num}) not found");
                }
                if (card_account.client_id != client.id){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(MakeCardTransfer)}) Client(ID={user_id}) doesn't own the account(ID={card_account.id}) from card(ID={card_num})");
                    return BadRequest($"Card(ID={card_num}) isn't owned by client(ID={user_id})");
                }
                // >> Registrar el movimiento para la tarjeta
                tecbankService.Movement_New_WithCard(card_num, transfer);
                return CreatedAtAction(nameof(MakeCardTransfer), new {id=transfer.id}, transfer);
            } catch (ServiceException e1){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeCardTransfer)}){e1.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (InvalidOperationException e2){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeCardTransfer)}){e2.ToString()}");
                return BadRequest($"Used credit card(ID={card_num}) doesn't have enough funds to proceed with transaction");
            } catch (ArgumentNullException e3){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeCardTransfer)}){e3.ToString()}");
                return BadRequest("Movement information is not valid");
            } catch (ArgumentException e4){
                logService.Log_New(LogTypes.ERROR,$"(HTTP)(POST={nameof(MakeCardTransfer)}){e4.ToString()}");
                return BadRequest("Failed to add a new movement to the database");
            }
        }

        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        [HttpPut("{user_id}/profile/update")]
        public ActionResult<ClientAccount> UpdateClient(int user_id, [FromBody] ClientAccount client){
            if (user_id != client.id)
                return BadRequest("Client ID is not the same as the ID from body");
            try{
                tecbankService.Client_Update(client);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(PUT={nameof(UpdateClient)}) Client(ID={user_id}) has been correctly modified");
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

        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
        [HttpDelete("{user_id}/delete")]
        public ActionResult RemoveClient(int user_id){
            try{
                tecbankService.Client_Delete(user_id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(RemoveClient)}) Removed client(ID={user_id}) successfully from database");
                return Ok($"Client(ID={user_id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveClient)}){e1.ToString()}");
                return NotFound($"Client(ID={user_id}) doesn't exist in the database");
            } catch (InvalidOperationException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveClient)}){e2.ToString()}");
                return BadRequest($"Client(ID={user_id}) has debts to pay");
            } catch (ServiceException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveClient)}){e3.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("{user_id}/accounts/{account_id}/close")]
        public ActionResult CloseAccount(int user_id, String account_id){
            try{
                var owner_account = tecbankService.Account_Get(account_id);
                if (owner_account == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(CloseAccount)}) Bank account(ID={account_id}) from card doesn't exist on the database");
                    return NotFound($"Bank account(ID={account_id}) doesn't exist");
                } else {
                    if (owner_account.client_id != user_id){
                        logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(CloseAccount)}) Bank account(ID={account_id}) doesn't belong to client(ID={user_id}) in the database");
                        return BadRequest($"Client(ID={user_id}) isn't the owner of the bank account(ID={account_id})");
                    }
                }
                tecbankService.Account_Delete(account_id);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(CloseAccount)}) Removed bank account(ID={account_id}) successfully from database");
                return Ok($"Bank account(ID={account_id}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(CloseAccount)}){e1.ToString()}");
                return NotFound($"Bank account(ID={account_id}) or client(ID={user_id}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(CloseAccount)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            }
        }

        [HttpDelete("{user_id}/cards/{card_num}/remove")]
        public ActionResult RemoveCard(int user_id, int card_num){
            try{
                var card = tecbankService.Card_Get(card_num);
                if (card == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveCard)}) Card(ID={card_num}) doesn't exist on database");
                    return NotFound($"Card(ID={card_num}) not found");
                }

                var card_account = tecbankService.Account_Get(card.account_id);
                if (card_account == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveCard)}) Account(ID={card.account_id}) from bank card(ID={card_num}) doesn't exist on database");
                    return NotFound($"Account(ID={card_num}) from bank card not found");
                }

                var client = tecbankService.Client_findByID(user_id);
                if (client == null){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(RemoveCard)}) Client(ID={user_id}) doesn't exist on database");
                    return NotFound($"Client(ID={user_id}) not found");
                }

                if (client.id != card_account.client_id){
                    logService.Log_New(LogTypes.ERROR, $"(HTTP)(POST={nameof(RemoveCard)}) Client(ID={user_id}) does not own bank account(ID={card_account.id})");
                    return BadRequest($"Client(ID={user_id}) does not own bank card(ID={card.card_num})");
                }

                tecbankService.Card_Delete(card_num);
                logService.Log_New(LogTypes.INFO, $"(HTTP)(DELETE={nameof(RemoveCard)}) Removed bank card(ID={card_num}) successfully from database");
                return Ok($"Bank card(ID={card_num}) removed successfully from system");
            } catch (KeyNotFoundException e1){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveCard)}){e1.ToString()}");
                return NotFound($"Bank account(ID={card_num}) doesn't exist on the database");
            }  catch (ServiceException e2){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveCard)}){e2.ToString()}");
                return StatusCode(500,"Internal server error");
            } catch (InvalidOperationException e3){
                logService.Log_New(LogTypes.ERROR, $"(HTTP)(DELETE={nameof(RemoveCard)}){e3.ToString()}");
                return BadRequest($"Card(ID={card_num}) has pending debts");
            }
        }
    }
}