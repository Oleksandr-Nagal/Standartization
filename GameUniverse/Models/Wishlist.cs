using System.ComponentModel.DataAnnotations.Schema;

namespace GameUniverse.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
