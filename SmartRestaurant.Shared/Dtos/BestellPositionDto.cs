namespace SmartRestaurant.Shared.Dtos;

public class BestellPositionDto
{
    public int BestellpositionId { get; set; }
    public int ArtikelId { get; set; }
    public string ArtikelName { get; set; } = "";
    public int Menge { get; set; }
    public decimal Einzelpreis { get; set; }
    public decimal Summe => Menge * Einzelpreis;
}