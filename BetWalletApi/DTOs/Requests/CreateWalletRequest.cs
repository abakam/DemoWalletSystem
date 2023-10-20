using System.ComponentModel.DataAnnotations;

namespace BetWalletApi.DTOs.Requests
{
    public class CreateWalletRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get;set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
