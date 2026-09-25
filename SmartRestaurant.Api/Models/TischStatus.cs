using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class TischStatus
{
    public uint TischStatusId { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Tisch> Tisch { get; set; } = new List<Tisch>();
}
