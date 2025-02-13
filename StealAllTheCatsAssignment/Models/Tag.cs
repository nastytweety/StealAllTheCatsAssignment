using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StealAllTheCatsAssignment.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<CatTag> CatTags { get; set; } = null!;

    }
}
