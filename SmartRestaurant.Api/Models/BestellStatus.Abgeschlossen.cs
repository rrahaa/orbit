namespace SmartRestaurant.Api.Models;

public partial class BestellStatus
{
    public const uint Serviert = 4;
    public const uint Bezahlt = 5;

    // Ab "Serviert" gilt eine Bestellung als abgeschlossen: Tisch wird frei, Umsatz zählt in der Statistik.
    public static readonly uint[] Abgeschlossen = { Serviert, Bezahlt };
}
