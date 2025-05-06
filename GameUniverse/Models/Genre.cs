using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}
