using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        public string? Website { get; set; }

        public string? Country { get; set; }
    }
}
