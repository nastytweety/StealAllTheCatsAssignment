using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    public class Cat : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string CatId { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = null!;
    }
}
