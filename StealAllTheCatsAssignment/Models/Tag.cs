using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Cat> Cats { get; set; } = null!;
    }
}
