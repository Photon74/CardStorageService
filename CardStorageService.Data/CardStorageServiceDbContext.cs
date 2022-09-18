using Microsoft.EntityFrameworkCore;

namespace CardStorageService.Data
{
    public class CardStorageServiceDbContext : DbContext
    {
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Client> Clients { get; set; }


        public CardStorageServiceDbContext(DbContextOptions options) : base(options) { }
    }
}
