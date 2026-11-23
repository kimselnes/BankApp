using BankApp.Data.Data;
using BankApp.Data.Interfaces;
using BankApp.Data.Repositories;
using BankApp.Services.Interfaces;
using BankApp.Services.Services;
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

            services.AddScoped<ISuspiciousTransactionRepository, SuspiciousTransactionRepository>();
            services.AddScoped<ISuspiciousTransactionService, SuspiciousTransactionService>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();
                Console.WriteLine($"Ansluten. Antal konton: {await db.Accounts.CountAsync()}");
                //Console.ReadLine();

                // TEST
                var fromDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-3);
                var service = scope.ServiceProvider.GetRequiredService<ISuspiciousTransactionService>();

                var rows = await service.GetSuspiciousTransactionsAsync(fromDate, 0);

                Console.WriteLine($"Rader totalt: {rows.Count}");
                Console.WriteLine($"Distinkta transaktionsnummer: {rows.Select(r => r.TransactionId).Distinct().Count()}");
                Console.WriteLine($"Länder: {string.Join(", ", rows.Select(r => r.Country).Distinct())}");

                foreach (var r in rows.Take(10))
                {
                    Console.WriteLine($"{r.TransactionId} | {r.AccountId} | {r.Date:yyyy-MM-dd} | {r.Amount:N2} | {r.CustomerName} | {r.Country} | {r.Rule}");
                }

                Console.ReadLine();
            }
        }
    }
}
