using TecBankAPI.Data;
using TecBankAPI.Models;

namespace TecBankAPI.Services;

public class CuentaService
{
    private readonly FileDataService _fileDataService;

    public CuentaService(FileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    public List<Cuenta> ObtenerTodas()
    {
        return _fileDataService.LeerCuentas();
    }

    public List<Cuenta> ObtenerPorCliente(string clienteId)
    {
        return _fileDataService.LeerCuentas()
            .Where(c => c.ClienteId == clienteId)
            .ToList();
    }

    public Cuenta? ObtenerPorNumero(string numero)
    {
        return _fileDataService.LeerCuentas()
            .FirstOrDefault(c => c.Numero == numero);
    }

    public void Crear(Cuenta cuenta)
    {
        var cuentas = _fileDataService.LeerCuentas();
        cuenta.Numero = GenerarNumeroCuenta();
        cuentas.Add(cuenta);
        _fileDataService.GuardarCuentas(cuentas);
    }

    public bool RealizarDeposito(string numeroCuenta, decimal monto, string? descripcion)
    {
        var cuentas = _fileDataService.LeerCuentas();
        var cuenta = cuentas.FirstOrDefault(c => c.Numero == numeroCuenta);
        
        if (cuenta == null) return false;
        
        cuenta.Saldo += monto;
        
        // Registrar movimiento
        var movimientos = _fileDataService.LeerMovimientos();
        movimientos.Add(new Movimiento
        {
            Id = Guid.NewGuid().ToString(),
            CuentaNumero = numeroCuenta,
            Fecha = DateTime.Now,
            Tipo = "Depósito",
            Monto = monto,
            Descripcion = descripcion
        });
        
        _fileDataService.GuardarCuentas(cuentas);
        _fileDataService.GuardarMovimientos(movimientos);
        
        return true;
    }

    private string GenerarNumeroCuenta()
    {
        return "CR" + DateTime.Now.Ticks.ToString()[^10..];
    }
}