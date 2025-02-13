using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    [Index(nameof(CatId), IsUnique = true)]
    public class Cat : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string CatId { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<CatTag> CatTags { get; set; } = null!;
    }
}
