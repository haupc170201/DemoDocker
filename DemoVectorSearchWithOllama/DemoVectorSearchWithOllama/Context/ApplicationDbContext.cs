using DemoVectorSearchWithOllama.Model;
using Microsoft.EntityFrameworkCore;


namespace DemoVectorSearchWithOllama.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(x => x.Embedding)
                .HasColumnType("vector(768)");
        }
    }
}
