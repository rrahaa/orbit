namespace SmartRestaurant.Shared.Dtos;

public class TischDto
{
    public int TischId { get; set; }
    public int Tischnummer { get; set; }
    public int Kapazitaet { get; set; }
    public string Status { get; set; } = "";
}