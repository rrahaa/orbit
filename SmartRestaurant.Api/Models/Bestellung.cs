using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Bestellung
{
    public uint BestellungId { get; set; }

    public uint TischId { get; set; }

    public uint MitarbeiterId { get; set; }

    public uint BestellStatusId { get; set; }

    public DateTime Bestelldatum { get; set; }

    public DateTime? Abschlusszeitpunkt { get; set; }

    public virtual BestellStatus BestellStatus { get; set; } = null!;

    public virtual ICollection<Bestellposition> Bestellposition { get; set; } = new List<Bestellposition>();

    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;

    public virtual Tisch Tisch { get; set; } = null!;
}
