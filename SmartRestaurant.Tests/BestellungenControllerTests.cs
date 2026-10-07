using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Controllers;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Tests;

public class BestellungenControllerTests
{
    [Fact]
    public async Task Create_OhnePositionen_IstBadRequest()
    {
        using var db = TestDb.Create();
        var controller = new BestellungenController(db, TestDb.CreateStatusLog());

        var result = await controller.Create(new NeueBestellungDto { TischId = 1, MitarbeiterId = 1 });

        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Empty(db.Bestellung);
    }

    [Fact]
    public async Task Create_SpeichertBestellungMitPreisAusDbUndBesetztTisch()
    {
        using var db = TestDb.Create();
        var controller = new BestellungenController(db, TestDb.CreateStatusLog());

        var result = await controller.Create(new NeueBestellungDto
        {
            TischId = 1,
            MitarbeiterId = 1,
            Positionen =
            {
                new NeueBestellPositionDto { ArtikelId = 1, Menge = 2 }, // 2 x Cola à 3,50
                new NeueBestellPositionDto { ArtikelId = 2, Menge = 1 }  // 1 x Bier à 4,20
            }
        });

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var dto = Assert.IsType<BestellungDto>(created.Value);
        Assert.Equal("Offen", dto.Status);
        Assert.Equal(11.20m, dto.Gesamtsumme);
        Assert.Equal(2u, db.Tisch.Single(t => t.TischId == 1).TischStatusId); // besetzt
    }

    [Fact]
    public async Task SetStatus_Bezahlt_GibtTischFreiUndSchreibtStatusLog()
    {
        using var db = TestDb.Create();
        var statusLog = TestDb.CreateStatusLog();
        var controller = new BestellungenController(db, statusLog);
        var created = (CreatedAtActionResult)await controller.Create(new NeueBestellungDto
        {
            TischId = 1,
            MitarbeiterId = 1,
            Positionen = { new NeueBestellPositionDto { ArtikelId = 1, Menge = 1 } }
        });
        var bestellungId = (uint)((BestellungDto)created.Value!).BestellungId;

        var result = await controller.SetStatus(bestellungId, new StatusAendernDto { NeuerStatusId = 5, MitarbeiterId = 1 });

        Assert.IsType<NoContentResult>(result);
        var bestellung = db.Bestellung.Single(b => b.BestellungId == bestellungId);
        Assert.NotNull(bestellung.Abschlusszeitpunkt);
        Assert.Equal(1u, db.Tisch.Single(t => t.TischId == 1).TischStatusId); // wieder frei

        var eintrag = Assert.Single(statusLog.AlleLesen());
        Assert.Equal("Offen", eintrag.AlterStatus);
        Assert.Equal("Bezahlt", eintrag.NeuerStatus);
        Assert.Equal("Anna Kellner", eintrag.MitarbeiterName);
    }

    [Fact]
    public async Task SetStatus_Serviert_GibtTischFrei()
    {
        using var db = TestDb.Create();
        var controller = new BestellungenController(db, TestDb.CreateStatusLog());
        var bestellungId = await NeueBestellung(controller, tischId: 1);

        var result = await controller.SetStatus(bestellungId, new StatusAendernDto { NeuerStatusId = 4, MitarbeiterId = 1 });

        Assert.IsType<NoContentResult>(result);
        Assert.NotNull(db.Bestellung.Single(b => b.BestellungId == bestellungId).Abschlusszeitpunkt);
        Assert.Equal(1u, db.Tisch.Single(t => t.TischId == 1).TischStatusId); // wieder frei
    }

    [Fact]
    public async Task SetStatus_Fertig_TischBleibtBesetzt()
    {
        using var db = TestDb.Create();
        var controller = new BestellungenController(db, TestDb.CreateStatusLog());
        var bestellungId = await NeueBestellung(controller, tischId: 1);

        await controller.SetStatus(bestellungId, new StatusAendernDto { NeuerStatusId = 3, MitarbeiterId = 1 });

        Assert.Equal(2u, db.Tisch.Single(t => t.TischId == 1).TischStatusId); // noch besetzt
    }

    [Fact]
    public async Task SetStatus_Serviert_TischBleibtBesetztSolangeAndereBestellungOffen()
    {
        using var db = TestDb.Create();
        var controller = new BestellungenController(db, TestDb.CreateStatusLog());
        var erste = await NeueBestellung(controller, tischId: 1);
        var zweite = await NeueBestellung(controller, tischId: 1);

        await controller.SetStatus(erste, new StatusAendernDto { NeuerStatusId = 4, MitarbeiterId = 1 });
        Assert.Equal(2u, db.Tisch.Single(t => t.TischId == 1).TischStatusId); // zweite noch offen

        await controller.SetStatus(zweite, new StatusAendernDto { NeuerStatusId = 4, MitarbeiterId = 1 });
        Assert.Equal(1u, db.Tisch.Single(t => t.TischId == 1).TischStatusId);
    }

    private static async Task<uint> NeueBestellung(BestellungenController controller, int tischId)
    {
        var created = (CreatedAtActionResult)await controller.Create(new NeueBestellungDto
        {
            TischId = tischId,
            MitarbeiterId = 1,
            Positionen = { new NeueBestellPositionDto { ArtikelId = 1, Menge = 1 } }
        });
        return (uint)((BestellungDto)created.Value!).BestellungId;
    }
}
