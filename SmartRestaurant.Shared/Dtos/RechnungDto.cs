namespace SmartRestaurant.Shared.Dtos;

public class RechnungDto
{
    public int TischId { get; set; }
    public int Tischnummer { get; set; }
    public List<BestellungDto> Bestellungen { get; set; } = new();
    public decimal Gesamtsumme => Bestellungen.Sum(b => b.Gesamtsumme);
}