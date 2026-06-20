
namespace Schuldenbuch.Core.DTOs.DebtDtos
{

    public enum DeleteStatus
    {
        Success,
        NotFound
    }

    public class DeleteDebtResultDto
    {
        public DeleteStatus Status { get; set; }
        public string Message { get; set; }
    }
}