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
        // ------------------------------------------------- [ POST ] -------------------------------------------------
        // ------------------------------------------------- [ PUT ] -------------------------------------------------
        // ------------------------------------------------- [ DELETE ] -------------------------------------------------
    }
}