namespace SmartRestaurant.Shared.Dtos;

public class StatistikDto
{
    public decimal Wochenumsatz { get; set; }
    public string? MeistverkauftesGetraenk { get; set; }
    public int MeistverkaufteMenge { get; set; }
}