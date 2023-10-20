namespace BetWalletApi.DTOs.Responses
{
    public class CreateWalletResponse
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid WalletId { get; set; }
        public Decimal Balance { get;set; }
    }
}
