namespace StealAllTheCatsAssignment.DTOs
{
    /// <summary>
    /// Skip the Image property because of the delay it introduces
    /// </summary>
    public record CatDto
    {
        public string CatId { get; set; } = String.Empty;
        public int Height { get; set; }
        public int Width { get; set; }  
        public DateTime Created { get; set; }
    }
}
