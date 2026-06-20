


using Schuldenbuch.Core.DTOs.DebtDtos;

public class CreatePdfDto
{
    public string Name {get;set;}
    public string Street {get; set;}
    public string ZipCode {get; set;}
    public string City {get; set;}
    public DateTime Date {get; set;}

    public decimal TotalAmount {get; set;}
    
    public List<DebtListItemDto> Debts{get; set;}
}