namespace BankAPI.Core.Dtos;

public class EventDto
{
    public string? Type { get; set; }
    public string? Destination { get; set; }
    public string? Origin { get; set; }
    public decimal Amount { get; set; }
}
