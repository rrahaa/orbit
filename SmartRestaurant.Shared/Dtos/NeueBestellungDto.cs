namespace SmartRestaurant.Shared.Dtos;

public class NeueBestellungDto
{
    public int TischId { get; set; }
    public int MitarbeiterId { get; set; }
    public List<NeueBestellPositionDto> Positionen { get; set; } = new();
}

public class NeueBestellPositionDto
{
    public int ArtikelId { get; set; }
    public int Menge { get; set; }
}