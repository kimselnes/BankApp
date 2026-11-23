namespace BankApp.SuspiciousTransactions
{
    public class ReportWriter
    {
        private readonly string _reportpath;
        private readonly string _stateFilePath;
        public ReportWriter(string reportpath, string stateFilePath)
        {
            _reportpath = reportpath;
            _stateFilePath = stateFilePath;
        }

    }
}
