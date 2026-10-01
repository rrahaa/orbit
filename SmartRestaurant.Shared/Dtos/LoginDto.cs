using System;
namespace SmartRestaurant.Shared.Dtos;

public class LoginDto
{
    public string Benutzername { get; set; } = string.Empty;
    public string Passwort { get; set; } = string.Empty;
}
