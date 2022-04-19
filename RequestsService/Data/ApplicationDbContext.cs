using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RequestsService.Models;

namespace RequestsService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<WebService> WebServices { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebService>().HasData(
            new WebService
            {
                Id = 1,
                Name = "VK",
                Url = "https://vk.com"
            },
            new WebService
            {
                Id = 2,
                Name = "Google",
                Url = "https://google.ru"
            },
            new WebService
            {
                Id = 3,
                Name = "Yandex",
                Url = "https://yandex.ru"
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
