namespace SmartRestaurant.Shared.Dtos;

public class NeuerMitarbeiterDto
{
    public string Name { get; set; } = "";
    public string Benutzername { get; set; } = "";
    public string Passwort { get; set; } = "";
    public int RolleId { get; set; }
}