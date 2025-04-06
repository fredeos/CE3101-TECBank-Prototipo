namespace TecBankAPI.Models;

public class Movimiento
{
    public string? Id { get; set; }
    public string? CuentaNumero { get; set; }
    public DateTime Fecha { get; set; }
    public string? Tipo { get; set; } // "Depósito", "Retiro"
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
}