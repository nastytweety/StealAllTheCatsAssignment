using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Domain.Models
{
    public class CatTag
    {
        [Key]
        public int Id { get; set; }
        public int CatId { get; set; }
        public int TagId { get; set; }
        public Cat Cat { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
