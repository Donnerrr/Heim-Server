

    namespace Schuldenbuch.Core.DTOs.DebtDtos
{

    public enum DebtResult
    {
        Success,
        NotFound
    }

    public class GetDebtResultDto
    {
        public DebtResult Status { get; set; }
        public string Message { get; set; }
    }
}