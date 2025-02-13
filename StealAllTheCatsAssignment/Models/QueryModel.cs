using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{

    public class QueryModel
    {
        /// <summary>
        /// The tagname as a string with a maximum 20 char length (optional)
        /// </summary>
        [StringLength(20,MinimumLength = 1)]
        public string? tag { get; set; }
        /// <summary>
        /// The page number as an integer >0 (required) 
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int page { get; set; }
        /// <summary>
        /// The page size as an integer higher than 0 and lower than 25 (required)
        /// </summary>
        [Required]
        [Range(1, 25)]
        public int pageSize { get; set; }
    }
}
