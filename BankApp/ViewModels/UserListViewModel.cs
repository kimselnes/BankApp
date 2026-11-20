namespace BankApp.ViewModels
{
    public class UserListViewModel
    {
        public List<UserRow> Users { get; set; } = new();

        public class UserRow
        {
            public string Id { get; set; } = null!;
            public string Email { get; set; } = null!;
            public List<string> Roles { get; set; } = new();
        }
    }
}
