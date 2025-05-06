using System.ComponentModel.DataAnnotations;

namespace GameUniverse.Models
{
    public class Game
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public required string Developer { get; set; }
        public required string Publisher { get; set; }
        public required string ImageUrl { get; set; }
        public required string Platform { get; set; }
    }
}
