namespace BankApp.Data.Models
{
    public enum TransactionError
    {
        None,
        AccountNotFound,
        TargetAccountNotFound,
        BalanceTooLow,
        IncorrectAmount,
        SameAccount
    }

    public record TransactionResult(bool Success, TransactionError Error, string? Message)
    {
        public static TransactionResult Ok() => new TransactionResult(true, TransactionError.None, null);

        public static TransactionResult Fail(TransactionError error, string message) => new TransactionResult(false, error, message);
    }
}
