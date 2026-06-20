namespace Schuldenbuch.Core.DTOs.PersonDtos
{
public enum GetPersonStatus
{
    Success,
    NotFound
}

public class GetPersonResultDto
{
    public GetPersonStatus Status { get; set; }
    public string Message { get; set; }
    public GetPersonDto Person { get; set; }
}
}