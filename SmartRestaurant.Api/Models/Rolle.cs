using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Rolle
{
    public uint RolleId { get; set; }

    public string Rollenname { get; set; } = null!;

    public virtual ICollection<Mitarbeiter> Mitarbeiter { get; set; } = new List<Mitarbeiter>();
}
