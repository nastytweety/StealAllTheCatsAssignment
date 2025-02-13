namespace StealAllTheCatsAssignment.DTOs
{
    /// <summary>
    /// Skip the Image property because of the delay it introduces
    /// </summary>
    public record CatDto
    {
        /// <summary>
        /// The CatId from thecatapi.com
        /// </summary>
        public string CatId { get; set; } = String.Empty;
        /// <summary>
        /// The image height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The image width
        /// </summary>
        public int Width { get; set; }  
        /// <summary>
        /// Date and time that added to database
        /// </summary>
        public DateTime Created { get; set; }
    }
}
