using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    public class Cat
    {
        [Key]
        public int Id { get; set; }
        public string CatId { get; set; } = null!;
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; } = null!;
        public virtual ICollection<Tag> Tags { get; set; } = null!;
    }
}
