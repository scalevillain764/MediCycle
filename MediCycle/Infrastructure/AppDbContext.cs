using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> AllUsers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Client>("Client")
            .HasValue<Worker>("Worker");

            // client with addresses
            builder.Entity<Client>() 
                .HasMany(c => c.Addresses)
                .WithOne(a => a.ClientOrganisation)
                .HasForeignKey(a => a.ClientId);

            // client with requests
            builder.Entity<Client>()
                .HasMany(c => c.Requests)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId);

            // address with requests
            builder.Entity<Address>()
                .HasMany(a => a.Requests)
                .WithOne(r => r.RequestAddress)
                .HasForeignKey(r => r.AddressId);

            // worker with request
            builder.Entity<Worker>()
                .HasMany(w => w.Requests)
                .WithOne(r => r.Executor)
                .HasForeignKey(r => r.ExecutorId);

        }
    }
}