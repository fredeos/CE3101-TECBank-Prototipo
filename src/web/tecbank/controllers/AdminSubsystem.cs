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
        // ------------------------------------------------- [ Specific GET ] -------------------------------------------------
        [HttpGet("clients/{id}")]
        public ActionResult<ClientAccount> GetClient(int id){
            var client = tecbankService.Client_findByID(id);
            if (client == null){
                return NotFound();
            }
            return Ok(client);
        }
        // ------------------------------------------------- [ POST ] -------------------------------------------------
        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
    }
}