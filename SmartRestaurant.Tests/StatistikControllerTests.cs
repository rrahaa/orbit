using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Controllers;
using SmartRestaurant.Api.Models;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Tests;

public class StatistikControllerTests
{
    [Fact]
    public async Task GetWoche_ZaehltNurBezahlteBestellungenDerLetzten7Tage()
    {
        using var db = TestDb.Create();
        db.Bestellung.AddRange(
            Bestellung(1, statusId: 5, tageAlt: 1, artikelId: 2, menge: 3),  // zählt: 3 x Bier
            Bestellung(2, statusId: 5, tageAlt: 2, artikelId: 1, menge: 1),  // zählt: 1 x Cola
            Bestellung(3, statusId: 1, tageAlt: 1, artikelId: 1, menge: 10), // offen -> zählt nicht
            Bestellung(4, statusId: 5, tageAlt: 30, artikelId: 1, menge: 10) // zu alt -> zählt nicht
        );
        await db.SaveChangesAsync();
        var controller = new StatistikController(db);

        var result = await controller.GetWoche();

        var dto = Assert.IsType<StatistikDto>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.Equal(3 * 4.20m + 3.50m, dto.Wochenumsatz);
        Assert.Equal("Bier", dto.MeistverkauftesGetraenk);
        Assert.Equal(3, dto.MeistverkaufteMenge);
    }

    private static Bestellung Bestellung(uint id, uint statusId, int tageAlt, uint artikelId, uint menge) => new()
    {
        BestellungId = id,
        TischId = 1,
        MitarbeiterId = 1,
        BestellStatusId = statusId,
        Bestelldatum = DateTime.Now.AddDays(-tageAlt),
        Bestellposition =
        {
            new Bestellposition
            {
                ArtikelId = artikelId,
                Menge = menge,
                Einzelpreis = artikelId == 1 ? 3.50m : 4.20m
            }
        }
    };
}
