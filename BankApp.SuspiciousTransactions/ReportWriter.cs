using BankApp.Data.DTOs;
using System.Text;

namespace BankApp.SuspiciousTransactions
{
    public class ReportWriter
    {
        private readonly string _reportpath;
        private readonly string _stateFilePath;
        public ReportWriter(string reportpath)
        {
            _reportpath = reportpath; 
            _stateFilePath = Path.Combine(reportpath, "last-run.txt");
        }

        public void WriteReport(string country, IList<SuspiciousTransactionDto> transactions)
        {
            var fileName = Path.Combine(_reportpath, $"{country}.txt");

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Misstänkta transaktioner - {country}");
            stringBuilder.AppendLine($"Genererad: {DateTime.Now:yyyy-MM-dd HH:mm}");
            stringBuilder.AppendLine($"Antal transaktioner: {transactions.Count}");
            stringBuilder.AppendLine(new string('-', 60));

            foreach (var t in transactions)
            {
                stringBuilder.AppendLine($"Person:            {t.CustomerName}");
                stringBuilder.AppendLine($"Kontonummer:       {t.AccountId}");
                stringBuilder.AppendLine($"Transaktionsnummer:{t.TransactionId}");
                stringBuilder.AppendLine($"Datum:             {t.Date:yyyy-MM-dd}");
                stringBuilder.AppendLine($"Belopp:            {t.Amount:N2} kr");
                stringBuilder.AppendLine($"Orsak:             {t.Rule}");
                stringBuilder.AppendLine();
            }

            File.WriteAllText(fileName, stringBuilder.ToString(), Encoding.UTF8);
        }

        public int ReadLastTransactionId()
        {
            if (!File.Exists(_stateFilePath))
            {
                return 0;
            }

            var text = File.ReadAllText(_stateFilePath).Trim();

            return Int32.TryParse(text, out var id) ? id : 0;
        }

        public void SaveLastTransactionId(int transactionId)
        {
            File.WriteAllText(_stateFilePath, transactionId.ToString(), Encoding.UTF8);
        }

    }
}
