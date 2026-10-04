namespace SmartRestaurant.Web.Auth;

// Rollennamen wie in der Tabelle `rolle` (Groß-/Kleinschreibung beachten)
public static class Rollen
{
    public const string Service = "Service";
    public const string Bar = "Bar";
    public const string Administration = "Administration";

    // Startseite, auf die ein Benutzer nach dem Login geleitet wird
    public static string Startseite(string? rolle) => rolle switch
    {
        Service => "/service",
        Bar => "/bar",
        Administration => "/admin",
        _ => "/login"
    };
}
