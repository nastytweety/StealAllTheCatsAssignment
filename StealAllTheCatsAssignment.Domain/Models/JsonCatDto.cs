using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Domain.Models
{
    public record JsonCatDto
    {
        [Required]
        public string id {  get; set; } = String.Empty;
        [Required]
        public string url { get; set; } = String.Empty;
        [Required]
        public int width { get; set; }
        [Required]
        public int height { get; set; }
        [Required]
        public List<Breed> breeds { get; set; } = null!;
    } 

    public class Breed
    {
        public string temperament { get; set; } = String.Empty;
    }

}
