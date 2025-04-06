using Microsoft.AspNetCore.Mvc;
using TecBankAPI.Models;
using TecBankAPI.Services;

namespace TecBankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public ActionResult<List<Cliente>> Get()
    {
        return _clienteService.ObtenerTodos();
    }

    [HttpGet("{id}")]
    public ActionResult<Cliente?> Get(string id)
    {
        var cliente = _clienteService.ObtenerPorId(id);
        
        if (cliente == null)
        {
            return NotFound();
        }
        
        return cliente;
    }

    [HttpPost]
    public IActionResult Post(Cliente cliente)
    {
        _clienteService.Crear(cliente);
        return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, Cliente cliente)
    {
        if (!_clienteService.Actualizar(id, cliente))
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        if (!_clienteService.Eliminar(id))
        {
            return NotFound();
        }
        
        return NoContent();
    }
}