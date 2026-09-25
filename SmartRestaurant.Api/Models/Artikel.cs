using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class Artikel
{
    public uint ArtikelId { get; set; }

    public uint KategorieId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Preis { get; set; }

    public virtual ICollection<Bestellposition> Bestellposition { get; set; } = new List<Bestellposition>();

    public virtual ArtikelKategorie Kategorie { get; set; } = null!;
}
