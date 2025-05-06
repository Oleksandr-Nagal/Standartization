using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public required string Manufacturer { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }
    }
}