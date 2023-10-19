using Microsoft.EntityFrameworkCore;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Common.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BetWalletApi.Repositories.EFCore
{
    public class BetWalletDbContext : DbContext
    {

        public BetWalletDbContext(DbContextOptions<BetWalletDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)  // Username is unique
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)    // Email is unique
                .IsUnique();
            modelBuilder.Entity<User>()   
                .HasOne(u => u.Wallet)    // one-user-to-one-wallet
                .WithOne(u => u.User)
                .HasForeignKey<Wallet>(w => w.UserId);
            modelBuilder.Entity<User>()   
                .HasMany(u => u.Transactions)  // one-user-to-many-transactions
                .WithOne(u => u.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Ledger)    // one-transaction-to-one-ledger
                .WithOne(t => t.Transaction)
                .HasForeignKey<Ledger>(l => l.TransactionId);
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionReference)
                .IsUnique();
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.PostToLedger);   // index PostToLedger column since it's frequently searched
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionType);
            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionType)
                .HasConversion<string>(new EnumToStringConverter<TransactionType>());
            modelBuilder.Entity<Transaction>()
                .Property(t => t.PostToLedger)
                .HasConversion<string>(new EnumToStringConverter<PostTransactionToLedger>());
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Ledger>()
                .Property(l => l.TransactionType)
                .HasConversion<string>(new EnumToStringConverter<WalletFlowType>());
            modelBuilder.Entity<Ledger>()
                .Property(l => l.Credit)
                .HasPrecision(19, 4);
            modelBuilder.Entity<Ledger>()
                .Property(l => l.Debit)
                .HasPrecision(19, 4);
            modelBuilder.Entity<Ledger>()
                .Property(l => l.CurrentBalance) 
                .HasPrecision(19, 4);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasPrecision(19, 4);
            modelBuilder.Entity<Wallet>()  // Optimistic concurrency: instead of locking the wallet table 
                .Property(w => w.Balance)  // to prevent another transaction from modifying the wallet balance,
                .IsConcurrencyToken();     // the original version of the wallet record retrieve from the database is compared to the version currently in the database,
                                           // if there are different, the update fails. These prevents concurrent transactions from modifying the wallet balance.

        }

    }
}
