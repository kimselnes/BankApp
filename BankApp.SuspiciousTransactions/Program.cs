using BankApp.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.SuspiciousTransactions
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<BankAppDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();
                Console.WriteLine($"Ansluten. Antal konton: {await db.Accounts.CountAsync()}");
                Console.ReadLine();
            }

        }
    }
}
