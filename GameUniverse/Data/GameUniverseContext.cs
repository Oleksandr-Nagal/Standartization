using GameUniverse.Models;
using Microsoft.EntityFrameworkCore;

namespace GameUniverse.Data
{
    public class GameUniverseContext : DbContext
    {
        public GameUniverseContext(DbContextOptions<GameUniverseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  
        }
    }
}
