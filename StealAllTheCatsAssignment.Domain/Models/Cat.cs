using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Domain.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(CatId), IsUnique = true)]
    public class Cat : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string CatId { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; } = null!;
        public virtual ICollection<CatTag> CatTags { get; set; } = null!;
    }
}
