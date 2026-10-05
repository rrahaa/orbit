namespace SmartRestaurant.Shared.Dtos;

public class StatusLogEintragDto
{
    public int BestellungId { get; set; }
    public int TischId { get; set; }
    public int MitarbeiterId { get; set; }
    public string MitarbeiterName { get; set; } = "";
    public string AlterStatus { get; set; } = "";
    public string NeuerStatus { get; set; } = "";
    public DateTime Zeitstempel { get; set; }
}