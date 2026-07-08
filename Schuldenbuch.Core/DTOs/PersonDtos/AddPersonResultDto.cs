namespace Schuldenbuch.Core.DTOs.PersonDtos;

public enum AddPersonStatus
{
	Success,
	ValidationError
}

public class AddPersonResultDto
{
	public AddPersonStatus Status {get; set;}
	public int Id {get; set;}
	public string Message {get; set;}
}
