namespace StealAllTheCatsAssignment.Application.DTOs
{
    public record ResponseDto
    {
        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; } = String.Empty;
        /// <summary>
        /// Response status
        /// </summary>
        public string Status { get; set; } = String.Empty;
    }
}
