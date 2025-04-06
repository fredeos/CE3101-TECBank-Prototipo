using TecBankAPI.Data;
using TecBankAPI.Models;

namespace TecBankAPI.Services;

public class ClienteService
{
    private readonly FileDataService _fileDataService;

    public ClienteService(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    public List<Cliente> ObtenerTodos()
    {
        return _fileDataService.LeerClientes();
    }

    public Cliente? ObtenerPorId(string id)
    {
        return _fileDataService.LeerClientes().FirstOrDefault(c => c.Id == id);
    }

    public void Crear(Cliente cliente)
    {
        var clientes = _fileDataService.LeerClientes();
        cliente.Id = Guid.NewGuid().ToString();
        clientes.Add(cliente);
        _fileDataService.GuardarClientes(clientes);
    }

    public bool Actualizar(string id, Cliente clienteActualizado)
    {
        var clientes = _fileDataService.LeerClientes();
        var cliente = clientes.FirstOrDefault(c => c.Id == id);
        
        if (cliente == null) return false;
        
        cliente.NombreCompleto = clienteActualizado.NombreCompleto;
        cliente.Cedula = clienteActualizado.Cedula;
        cliente.Direccion = clienteActualizado.Direccion;
        cliente.Telefono = clienteActualizado.Telefono;
        cliente.IngresoMensual = clienteActualizado.IngresoMensual;
        cliente.TipoCliente = clienteActualizado.TipoCliente;
        
        _fileDataService.GuardarClientes(clientes);
        return true;
    }

    public bool Eliminar(string id)
    {
        var clientes = _fileDataService.LeerClientes();
        var cliente = clientes.FirstOrDefault(c => c.Id == id);
        
        if (cliente == null) return false;
        
        clientes.Remove(cliente);
        _fileDataService.GuardarClientes(clientes);
        return true;
    }
}