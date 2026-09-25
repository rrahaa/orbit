using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Bestellposition
{
    public uint BestellpositionId { get; set; }

    public uint BestellungId { get; set; }

    public uint ArtikelId { get; set; }

    public uint Menge { get; set; }

    public decimal Einzelpreis { get; set; }

    public virtual Artikel Artikel { get; set; } = null!;

    public virtual Bestellung Bestellung { get; set; } = null!;
}
