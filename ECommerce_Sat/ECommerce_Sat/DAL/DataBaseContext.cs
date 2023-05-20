using ECommerce_Sat.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.DAL
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        //Aquí estoy mapeando mi entidad para convertirla en un DBSet (tabla)
        public DbSet<Country> Countries { get; set; } //La tabla se debe llamar en plural: Countries
        public DbSet<Category> Categories { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TemporalSale> TemporalSales { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        //Vamos a crear un índice para la tabla Countries
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique(); // Para estos casos, debo crear un índice Compuesto
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique(); // Para estos casos, debo crear un índice Compuesto
            modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<ProductCategory>().HasIndex("ProductId", "CategoryId").IsUnique();
        }
    }
}
