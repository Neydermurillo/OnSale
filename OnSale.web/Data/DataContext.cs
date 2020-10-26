using Microsoft.EntityFrameworkCore;
using OnSaleCommon.Entities;

namespace OnSale.web.Data
{
    public class DataContext : DbContext
    {
        //el constructor se conecta a la base de datos 
        public DataContext(DbContextOptions<DataContext> options) : base(options) //se le pasa el parametro a la clase base
        {

        }
        //mapeo de tabla en la basde de datos

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> countries { get; set; }

        public DbSet<Department> Departments { get; set; }

        //en este metodo se crea un indice para que no se repita los nombres de los paises, el indice es unico 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
            .HasIndex(t => t.Name)
            .IsUnique();

            modelBuilder.Entity<City>()
          .HasIndex(t => t.Name)
          .IsUnique();

            modelBuilder.Entity<Department>()
          .HasIndex(t => t.Name)
          .IsUnique();

        }
    }
}

