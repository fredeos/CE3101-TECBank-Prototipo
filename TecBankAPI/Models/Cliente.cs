namespace TecBankAPI.Models;

public class Cliente
{
    
    public string? Id { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Cedula { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public decimal IngresoMensual { get; set; }
    public string? TipoCliente { get; set; } // "Físico" o "Jurídico"
    public string? Usuario { get; set; }
    public string? Password { get; set; }
}