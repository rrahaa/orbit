using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Controllers;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Tests;

// Validierung der Admin-Funktionen für Artikel und Tische.
public class StammdatenTests
{
    [Fact]
    public async Task ArtikelCreate_MitNegativemPreis_IstBadRequest()
    {
        using var db = TestDb.Create();
        var controller = new ArtikelController(db);

        var result = await controller.Create(new NeuerArtikelDto { Name = "Wasser", Preis = -1m, KategorieId = 1 });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ArtikelCreate_GleicherNameInKategorie_IstConflict()
    {
        using var db = TestDb.Create();
        var controller = new ArtikelController(db);

        // "cola" existiert schon als "Cola" -> Vergleich ignoriert Groß-/Kleinschreibung
        var result = await controller.Create(new NeuerArtikelDto { Name = "  cola ", Preis = 3m, KategorieId = 1 });

        Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(2, db.Artikel.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(9)]
    public async Task TischCreate_MitUngueltigerKapazitaet_IstBadRequest(int kapazitaet)
    {
        using var db = TestDb.Create();
        var controller = new TischeAdminController(db);

        var result = await controller.Create(new NeuerTischDto { Tischnummer = 10, Kapazitaet = kapazitaet });

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
