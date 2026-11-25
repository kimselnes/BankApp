namespace BankApp.Data.DTOs
{
    public record CountryStatsDto(
        string Country,
        int CustomerCount,
        int AccountCount,
        decimal TotalBalance);
}
