using Microsoft.EntityFrameworkCore;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;

namespace BetWalletApi.Repositories.EFCore
{
    public class BetWalletDbContext : DbContext
    {

        public BetWalletDbContext(DbContextOptions<BetWalletDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }

        // Override OnModelCreating to handle Enum to String Conversion for Enum Type fields
        // Also consider setting Id on Creation
        // Also consider fixing conditional update for Balance update

    }
}
