using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SmartRestaurant.Api.Data;
using SmartRestaurant.Api.Models;
using SmartRestaurant.Api.Services;

namespace SmartRestaurant.Tests;

// Erstellt für jeden Test eine eigene, frische In-Memory-Datenbank mit Stammdaten.
// So laufen die Tests ohne MySQL-Server und beeinflussen sich nicht gegenseitig.
public static class TestDb
{
    public const string KellnerPasswort = "geheim123";

    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new AppDbContext(options);
        Seed(db);
        return db;
    }

    // StatusLogService schreibt in eine Datei -> im Test in einen eigenen temporären Ordner.
    public static StatusLogService CreateStatusLog()
    {
        var ordner = Path.Combine(Path.GetTempPath(), "orbit-tests", Guid.NewGuid().ToString());
        return new StatusLogService(new TestEnvironment { ContentRootPath = ordner });
    }

    private class TestEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "SmartRestaurant.Tests";
        public string EnvironmentName { get; set; } = "Test";
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = "";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    }

    private static void Seed(AppDbContext db)
    {
        db.Rolle.AddRange(
            new Rolle { RolleId = 1, Rollenname = "Admin" },
            new Rolle { RolleId = 2, Rollenname = "Service" });

        db.TischStatus.AddRange(
            new TischStatus { TischStatusId = 1, Statusname = "frei" },
            new TischStatus { TischStatusId = 2, Statusname = "besetzt" });

        db.BestellStatus.AddRange(
            new BestellStatus { BestellStatusId = 1, Statusname = "Offen" },
            new BestellStatus { BestellStatusId = 2, Statusname = "In Bearbeitung" },
            new BestellStatus { BestellStatusId = 5, Statusname = "Bezahlt" });

        db.ArtikelKategorie.Add(new ArtikelKategorie { KategorieId = 1, Kategoriename = "Getränke" });

        db.Artikel.AddRange(
            new Artikel { ArtikelId = 1, KategorieId = 1, Name = "Cola", Preis = 3.50m },
            new Artikel { ArtikelId = 2, KategorieId = 1, Name = "Bier", Preis = 4.20m });

        db.Tisch.AddRange(
            new Tisch { TischId = 1, Tischnummer = 1, Kapazitaet = 4, TischStatusId = 1 },
            new Tisch { TischId = 2, Tischnummer = 2, Kapazitaet = 2, TischStatusId = 1 });

        db.Mitarbeiter.AddRange(
            new Mitarbeiter
            {
                MitarbeiterId = 1, RolleId = 2, Name = "Anna Kellner", Benutzername = "anna",
                PasswortHash = BCrypt.Net.BCrypt.HashPassword(KellnerPasswort), IstAktiv = true
            },
            new Mitarbeiter
            {
                MitarbeiterId = 2, RolleId = 2, Name = "Max Ehemalig", Benutzername = "max",
                PasswortHash = BCrypt.Net.BCrypt.HashPassword(KellnerPasswort), IstAktiv = false
            });

        db.SaveChanges();
        db.ChangeTracker.Clear();
    }
}
