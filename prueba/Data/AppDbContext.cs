using Microsoft.EntityFrameworkCore;
using prueba.Entities;

namespace prueba.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Marca>().ToTable("Marcas");
            modelBuilder.Entity<Vehiculo>().ToTable("Vehiculos")
                .HasOne(v => v.Marca)
                .WithMany()
                .HasForeignKey(v => v.MarcaId);

            // Datos semilla para marcas de autos
            modelBuilder.Entity<Marca>().HasData(
                new Marca
                {
                    Id = 1,
                    Nombre = "Toyota",
                    Pais = "Japón",
                    AnioFundacion = 1937,
                    SedeCentral = "Toyota City, Japón",
                    EsMarcaLujo = false,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 2,
                    Nombre = "Volkswagen",
                    Pais = "Alemania",
                    AnioFundacion = 1937,
                    SedeCentral = "Wolfsburg, Alemania",
                    EsMarcaLujo = false,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 3,
                    Nombre = "Mercedes-Benz",
                    Pais = "Alemania",
                    AnioFundacion = 1926,
                    SedeCentral = "Stuttgart, Alemania",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 4,
                    Nombre = "BMW",
                    Pais = "Alemania",
                    AnioFundacion = 1916,
                    SedeCentral = "Múnich, Alemania",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 5,
                    Nombre = "Honda",
                    Pais = "Japón",
                    AnioFundacion = 1948,
                    SedeCentral = "Tokio, Japón",
                    EsMarcaLujo = false,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 6,
                    Nombre = "Ford",
                    Pais = "Estados Unidos",
                    AnioFundacion = 1903,
                    SedeCentral = "Dearborn, Michigan",
                    EsMarcaLujo = false,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 7,
                    Nombre = "Audi",
                    Pais = "Alemania",
                    AnioFundacion = 1909,
                    SedeCentral = "Ingolstadt, Alemania",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 8,
                    Nombre = "Porsche",
                    Pais = "Alemania",
                    AnioFundacion = 1931,
                    SedeCentral = "Stuttgart, Alemania",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 9,
                    Nombre = "Ferrari",
                    Pais = "Italia",
                    AnioFundacion = 1947,
                    SedeCentral = "Maranello, Italia",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                },
                new Marca
                {
                    Id = 10,
                    Nombre = "Lamborghini",
                    Pais = "Italia",
                    AnioFundacion = 1963,
                    SedeCentral = "Sant'Agata Bolognese, Italia",
                    EsMarcaLujo = true,
                    FechaCreacion = DateTime.Now
                }
            );

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Marca)
                .WithMany(m => m.Vehiculos)
                .HasForeignKey(v => v.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Additional configurations if needed
            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }



    }
}