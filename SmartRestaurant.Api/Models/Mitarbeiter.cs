using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Mitarbeiter
{
    public uint MitarbeiterId { get; set; }

    public uint RolleId { get; set; }

    public string Name { get; set; } = null!;

    public string Benutzername { get; set; } = null!;

    public string PasswortHash { get; set; } = null!;

    public bool? IstAktiv { get; set; }

    public virtual ICollection<Bestellung> Bestellung { get; set; } = new List<Bestellung>();

    public virtual Rolle Rolle { get; set; } = null!;
}
