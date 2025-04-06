using Microsoft.AspNetCore.Mvc;
using TecBankAPI.Models;
using TecBankAPI.Services;

namespace TecBankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    private readonly CuentaService _cuentaService;

    public CuentasController(CuentaService cuentaService)
    {
        _cuentaService = cuentaService;
    }

    [HttpGet]
    public ActionResult<List<Cuenta>> Get()
    {
        return _cuentaService.ObtenerTodas();
    }

    [HttpGet("por-cliente/{clienteId}")]
    public ActionResult<List<Cuenta>> GetByCliente(string clienteId)
    {
        return _cuentaService.ObtenerPorCliente(clienteId);
    }

    [HttpGet("{numero}")]
    public ActionResult<Cuenta?> Get(string numero)
    {
        var cuenta = _cuentaService.ObtenerPorNumero(numero);
        
        if (cuenta == null)
        {
            return NotFound();
        }
        
        return cuenta;
    }

    [HttpPost]
    public IActionResult Post(Cuenta cuenta)
    {
        _cuentaService.Crear(cuenta);
        return CreatedAtAction(nameof(Get), new { numero = cuenta.Numero }, cuenta);
    }

    [HttpPost("{numeroCuenta}/deposito")]
    public IActionResult Deposito(string numeroCuenta, [FromBody] DepositoRequest request)
    {
        if (!_cuentaService.RealizarDeposito(numeroCuenta, request.Monto, request.Descripcion))
        {
            return NotFound();
        }
        
        return NoContent();
    }
}

public class DepositoRequest
{
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
}