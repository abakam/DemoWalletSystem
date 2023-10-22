using BetWalletApi.Models.Common;

namespace BetWalletApi.Models.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
