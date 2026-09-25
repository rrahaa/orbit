using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class BestellStatus
{
    public uint BestellStatusId { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Bestellung> Bestellung { get; set; } = new List<Bestellung>();
}
