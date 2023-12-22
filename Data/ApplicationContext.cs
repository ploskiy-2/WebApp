using Microsoft.EntityFrameworkCore;

namespace WebAppForBD.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Human> Humans => Set<Human>();

        public DbSet<Tag> Tags => Set<Tag>();

        public ApplicationContext() => Database.EnsureCreated();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
            optionsBuilder.EnableSensitiveDataLogging();

        }
    }

}
