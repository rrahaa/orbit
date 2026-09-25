namespace SmartRestaurant.Shared.Dtos;

public class MitarbeiterDto
{
    public int MitarbeiterId { get; set; }
    public string Name { get; set; } = "";
    public string Benutzername { get; set; } = "";
    public string Rolle { get; set; } = "";
    public bool IstAktiv { get; set; }
}