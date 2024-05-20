namespace Presentation.WebApi.Data.DTOs.ApiErrors
{
    public class ApiErrorDTO
    {
        public required string message { get; set; }
        public object? content { get; set; }
    }
}
