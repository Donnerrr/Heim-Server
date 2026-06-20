namespace Schuldenbuch.Core.DTOs.DebtDtos;


public class DebtListItemDto
{
    public int Id {get; set;}
    public int PersonId {get; set;}
    public decimal Amount {get; set;}
    public string Reason {get; set;}
    public DateTime Date {get; set;}
}
