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

            // ReportFile config:
            var reportPath = configuration["ReportPath"];
            if (string.IsNullOrWhiteSpace(reportPath))
            {
                reportPath = Path.Combine(AppContext.BaseDirectory, "reports");
            }
            Directory.CreateDirectory(reportPath);


            var services = new ServiceCollection();

            services.AddDbContext<BankAppDataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISuspiciousTransactionRepository, SuspiciousTransactionRepository>();
            services.AddScoped<ISuspiciousTransactionService, SuspiciousTransactionService>();

            var serviceProvider = services.BuildServiceProvider();

            var writer = new ReportWriter(reportPath);

            using (var scope = serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ISuspiciousTransactionService>();

                var today = DateOnly.FromDateTime(DateTime.Now);
                var fromDate = today.AddDays(-3);

                var lastTransactionId = writer.ReadLastTransactionId();

                Console.WriteLine($"Rapportmapp: {reportPath}");
                Console.WriteLine($"Fönster: {fromDate:yyyy-MM-dd} till {today:yyyy-MM-dd}");
                Console.WriteLine($"Föregående körnings högsta transaktionsnummer: {lastTransactionId}");
                Console.WriteLine();

                var rows = await service.GetSuspiciousTransactionsAsync(fromDate, lastTransactionId);

                if (rows.Count == 0)
                {
                    Console.WriteLine("Inga misstänkta transaktioner i fönstret.");
                    Console.ReadLine();
                    return;
                }

                var byCountry = rows.GroupBy(r => r.Country).OrderBy(g => g.Key);

                var highestId = rows.Max(r => r.TransactionId);
                var newHighestId = Math.Max(lastTransactionId, highestId);

                foreach (var group in byCountry)
                {
                    writer.WriteReport(group.Key, group.ToList());
                    Console.WriteLine($"{group.Key}: {group.Count()} transaktioner -> {group.Key}.txt");
                }

                writer.SaveLastTransactionId(newHighestId);

                Console.WriteLine();
                Console.WriteLine($"Klart. Högsta rapporterade transaktionsnummer: {newHighestId}");
                Console.ReadLine();

                //var db = scope.ServiceProvider.GetRequiredService<BankAppDataContext>();
                //Console.WriteLine($"Ansluten. Antal konton: {await db.Accounts.CountAsync()}");
                //Console.ReadLine();

                // TEST - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                //var fromDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-3);
                //var service = scope.ServiceProvider.GetRequiredService<ISuspiciousTransactionService>();

                //var rows = await service.GetSuspiciousTransactionsAsync(fromDate, 0);

                //Console.WriteLine($"Rader totalt: {rows.Count}");
                //Console.WriteLine($"Distinkta transaktionsnummer: {rows.Select(r => r.TransactionId).Distinct().Count()}");
                //Console.WriteLine($"Länder: {string.Join(", ", rows.Select(r => r.Country).Distinct())}");

                //foreach (var r in rows.Take(10))
                //{
                //    Console.WriteLine($"{r.TransactionId} | {r.AccountId} | {r.Date:yyyy-MM-dd} | {r.Amount:N2} | {r.CustomerName} | {r.Country} | {r.Rule}");
                //}

                //Console.ReadLine();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            }
        }
    }
}
