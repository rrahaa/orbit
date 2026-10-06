using Microsoft.AspNetCore.Mvc;
using SmartRestaurant.Api.Controllers;
using SmartRestaurant.Shared.Dtos;

namespace SmartRestaurant.Tests;

public class LoginControllerTests
{
    [Fact]
    public async Task Login_MitRichtigemPasswort_LiefertMitarbeiterUndRolle()
    {
        using var db = TestDb.Create();
        var controller = new LoginController(db);

        var result = await controller.Login(new LoginDto { Benutzername = "anna", Passwort = TestDb.KellnerPasswort });

        var ok = Assert.IsType<OkObjectResult>(result);
        var antwort = Assert.IsType<LoginAntwortDto>(ok.Value);
        Assert.Equal(1, antwort.MitarbeiterId);
        Assert.Equal("Service", antwort.Rolle);
    }

    [Fact]
    public async Task Login_MitFalschemPasswort_IstUnauthorized()
    {
        using var db = TestDb.Create();
        var controller = new LoginController(db);

        var result = await controller.Login(new LoginDto { Benutzername = "anna", Passwort = "falsch" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_DeaktivierterMitarbeiter_IstUnauthorized()
    {
        using var db = TestDb.Create();
        var controller = new LoginController(db);

        var result = await controller.Login(new LoginDto { Benutzername = "max", Passwort = TestDb.KellnerPasswort });

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Dieser Benutzer ist deaktiviert.", unauthorized.Value);
    }
}
