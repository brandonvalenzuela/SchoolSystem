

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SchoolSystem.Infrastructure.Persistence.Context
    {
        public class SchoolSystemDbContextFactory : IDesignTimeDbContextFactory<SchoolSystemDbContext>
        {
            public SchoolSystemDbContext CreateDbContext(string[] args)
            {
                // La cadena de conexión es solo para que la herramienta de migración 
                // pueda construir el modelo, no necesita ser la cadena de producción.
                const string connectionString = "Server=localhost;Port=3306;Database=SchoolSystem;User=root;Password=12345;";

                var optionsBuilder = new DbContextOptionsBuilder<SchoolSystemDbContext>();

                // **IMPORTANTE:** Usa el mismo proveedor que usas en tu API (Pomelo.EntityFrameworkCore.MySql)
                optionsBuilder.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );

                return new SchoolSystemDbContext(optionsBuilder.Options);
            }
        }
    }