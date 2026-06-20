using Schuldenbuch.Core.DTOs.DebtDtos;

namespace Schuldenbuch.Core.DTOs.PersonDtos;



public class GetPersonDto
{
    public string Name {get; set;}
    public string Street {get; set;}
    public string ZipCode {get; set;}
    public string City {get; set;}

    public decimal Amount{get; set;}
    public List<DebtListItemDto> Debts{get; set;}
}
