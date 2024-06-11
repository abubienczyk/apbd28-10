// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
//
// namespace WebDoctorApp.Data;
//
// public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<Context>
// {
//     public Context CreateDbContext(string[] args)
//     {
//         // Zdefiniuj ścieżkę do pliku konfiguracyjnego appsettings.json
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json")
//             .Build();
//
//         // Pobierz connection string z konfiguracji
//         var connectionString = configuration.GetConnectionString("Default");
//
//         // Konfiguracja opcji DbContext
//         var optionsBuilder = new DbContextOptionsBuilder<Context>();
//         optionsBuilder.UseSqlServer(connectionString);
//
//         // Zwróć nową instancję MyContext
//         return new Context(optionsBuilder.Options);
//     }
//     
// }