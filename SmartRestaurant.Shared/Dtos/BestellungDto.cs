namespace SmartRestaurant.Shared.Dtos;

public class BestellungDto
{
    public int BestellungId { get; set; }
    public int TischId { get; set; }
    public int Tischnummer { get; set; }
    public string MitarbeiterName { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime Bestelldatum { get; set; }
    public DateTime? Abschlusszeitpunkt { get; set; }
    public List<BestellPositionDto> Positionen { get; set; } = new();
    public decimal Gesamtsumme => Positionen.Sum(p => p.Summe);
}