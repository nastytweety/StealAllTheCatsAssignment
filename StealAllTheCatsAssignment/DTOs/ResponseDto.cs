namespace StealAllTheCatsAssignment.DTOs
{
    public record ResponseDto
    {
        public string Message { get; set; } = String.Empty;

        public string Status { get; set; } = String.Empty;
    }
}
