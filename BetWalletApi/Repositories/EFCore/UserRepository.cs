using BetWalletApi.Models.Common;
using BetWalletApi.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace BetWalletApi.Repositories.EFCore
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly BetWalletDbContext _dbContext;
        public UserRepository(BetWalletDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbContext.Set<User>().Where(u => u.UserName == username).FirstOrDefaultAsync();
        }
    }
}
