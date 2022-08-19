using System.Data.Entity;

namespace WindowsFormsApp1.DataModel
{
    public class GameContext : DbContext
    {
        public GameContext()
            : base("name=GameContext")
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<CardCollection> CardCollections { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Searching> Searchings { get; set; }
        public DbSet<PlrInfo> PlrsInfo { get; set; }
    }
}