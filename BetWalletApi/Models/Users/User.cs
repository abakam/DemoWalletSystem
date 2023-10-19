using BetWalletApi.Models.Common;

namespace BetWalletApi.Models.Users
{
    public class User : Entity
    {
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
    }
}
