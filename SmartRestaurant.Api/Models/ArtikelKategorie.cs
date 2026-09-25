using System;
using System.Collections.Generic;

namespace SmartRestaurant.Api.Models;

public partial class ArtikelKategorie
{
    public uint KategorieId { get; set; }

    public string Kategoriename { get; set; } = null!;

    public virtual ICollection<Artikel> Artikel { get; set; } = new List<Artikel>();
}
