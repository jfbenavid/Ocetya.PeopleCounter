namespace GranEstacion.Repository
{
    using Microsoft.EntityFrameworkCore;

    public class GranEstacionContext : DbContext
    {
        public GranEstacionContext(DbContextOptions<GranEstacionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>().HasData(
                new Camera { CameraId = 1, Name = "Subida Rampa de Sotano 2 a Sotano 1" },
                new Camera { CameraId = 2, Name = "Salida Ascensor Sotano 1" },
                new Camera { CameraId = 3, Name = "Subida Escaleras Sotano 1 a Piso 1" },
                new Camera { CameraId = 4, Name = "Rampa Subida Sotano 1 a Piso 1" },
                new Camera { CameraId = 5, Name = "Rampa Bajada de Piso 1 a Sotano 1" },
                new Camera { CameraId = 6, Name = "Tunel Granestacion 1-2" },
                new Camera { CameraId = 7, Name = "Puerta 2 Costado Norte" },
                new Camera { CameraId = 8, Name = "Puerta 2 Costado Sur" },
                new Camera { CameraId = 9, Name = "Puerta 1 Costado Norte" },
                new Camera { CameraId = 10, Name = "Puerta 1 Costado Sur" },
                new Camera { CameraId = 11, Name = "Salida Ascensor Sotano 2" },
                new Camera { CameraId = 12, Name = "Bajada a Rampa a Sotano 2" }
            );
        }

        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}