using System;

namespace SmartRestaurant.Shared.Dtos;

public class LoginAntwortDto
{
    public int MitarbeiterId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rolle { get; set; } = string.Empty;
}
