namespace StealAllTheCatsAssignment.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Cat> Cats { get; set; } = null!;
    }
}
