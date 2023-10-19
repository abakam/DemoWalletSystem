using BetWalletApi.Models.Common;
using BetWalletApi.Models.Users;

namespace BetWalletApi.Repositories.EFCore
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
        }
    }
}
