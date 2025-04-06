using System.Xml.Serialization;
using TecBankAPI.Models;

namespace TecBankAPI.Data;

public class FileDataService
{
    private const string ClientesFile = "Clientes.xml";
    private const string CuentasFile = "Cuentas.xml";
    private const string MovimientosFile = "Movimientos.xml";

    public void InitializeFiles()
    {
        if (!File.Exists(ClientesFile))
        {
            GuardarClientes(new List<Cliente>());
        }
        
        if (!File.Exists(CuentasFile))
        {
            GuardarCuentas(new List<Cuenta>());
        }
        
        if (!File.Exists(MovimientosFile))
        {
            GuardarMovimientos(new List<Movimiento>());
        }
    }

    public List<Cliente> LeerClientes()
    {
        var serializer = new XmlSerializer(typeof(List<Cliente>));
        using var reader = new StreamReader(ClientesFile);
        return (List<Cliente>)serializer.Deserialize(reader)!;
    }

    public void GuardarClientes(List<Cliente> clientes)
    {
        var serializer = new XmlSerializer(typeof(List<Cliente>));
        using var writer = new StreamWriter(ClientesFile);
        serializer.Serialize(writer, clientes);
    }

    // Métodos similares para Cuentas y Movimientos...
    public List<Cuenta> LeerCuentas()
    {
        var serializer = new XmlSerializer(typeof(List<Cuenta>));
        using var reader = new StreamReader(CuentasFile);
        return (List<Cuenta>)serializer.Deserialize(reader)!;
    }

    public void GuardarCuentas(List<Cuenta> cuentas)
    {
        var serializer = new XmlSerializer(typeof(List<Cuenta>));
        using var writer = new StreamWriter(CuentasFile);
        serializer.Serialize(writer, cuentas);
    }

    public List<Movimiento> LeerMovimientos()
    {
        var serializer = new XmlSerializer(typeof(List<Movimiento>));
        using var reader = new StreamReader(MovimientosFile);
        return (List<Movimiento>)serializer.Deserialize(reader)!;
    }

    public void GuardarMovimientos(List<Movimiento> movimientos)
    {
        var serializer = new XmlSerializer(typeof(List<Movimiento>));
        using var writer = new StreamWriter(MovimientosFile);
        serializer.Serialize(writer, movimientos);
    }
}