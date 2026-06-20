namespace Schuldenbuch.Core.DTOs.PersonDtos;



public enum DeleteStatus
{
    Success,
    NotFound,
    NeedsConfirmation
}


public class DeletePersonResultDto
{
    public  DeleteStatus Status {get; set;}
    public string Message {get; set;}
}
