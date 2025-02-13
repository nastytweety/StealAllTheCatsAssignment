using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    public class QueryModel
    {
        [StringLength(20,MinimumLength = 1)]
        public string? tag { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int page { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int pageSize { get; set; }
    }
}
