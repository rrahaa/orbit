namespace SmartRestaurant.Shared.Dtos;

public class ArtikelDto
{
    public int ArtikelId { get; set; }
    public string Name { get; set; } = "";
    public decimal Preis { get; set; }
    public string Kategorie { get; set; } = "";
}