﻿using ECommerce_Sat.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        //Aquí estoy mappeando mi entidad para convertirla en un DBSet (tabla)
        public DbSet<Country> Countries { get; set; } //La tabla se debe llamar en plural: Countries
        public DbSet<Category> Categories { get; set; } //La tabla se debe llamar en plural: Countries
        public DbSet<State> States { get; set; } //La tabla se debe llamar en plural: Countries

        //Vamos a crar un índice para la tabla Countries
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique(); // Para estos casos, debo crear un índice Compuesto
        }
    }
}
