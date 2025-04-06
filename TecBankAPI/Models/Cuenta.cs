namespace TecBankAPI.Models;

public class Cuenta
{
    public string? Numero { get; set; }
    public string? Descripcion { get; set; }
    public string? Moneda { get; set; } // "Colones", "Dólares", "Euros"
    public string? Tipo { get; set; } // "Ahorros", "Corriente"
    public string? ClienteId { get; set; }
    public decimal Saldo { get; set; }
}