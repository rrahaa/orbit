namespace SmartRestaurant.Shared.Dtos;

public class NeuerArtikelDto
{
    public string Name { get; set; } = "";
    public decimal Preis { get; set; }
    public int KategorieId { get; set; }
}