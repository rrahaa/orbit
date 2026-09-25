using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Tisch
{
    public uint TischId { get; set; }

    public uint TischStatusId { get; set; }

    public uint Tischnummer { get; set; }

    public uint Kapazitaet { get; set; }

    public virtual ICollection<Bestellung> Bestellung { get; set; } = new List<Bestellung>();

    public virtual TischStatus TischStatus { get; set; } = null!;
}
