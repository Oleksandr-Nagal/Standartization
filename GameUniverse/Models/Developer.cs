using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class Developer
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Website { get; set; }

        public string Country { get; set; }
    }
}
