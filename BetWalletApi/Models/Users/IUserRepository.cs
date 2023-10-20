using BetWalletApi.Models.Common;

namespace BetWalletApi.Models.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByUsernameAsync(string username);
    }
}
