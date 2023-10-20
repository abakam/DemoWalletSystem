using BetWalletApi.Models.Common;
using BetWalletApi.Models.Ledgers;
using BetWalletApi.Models.Transactions;
using BetWalletApi.Models.Users;
using BetWalletApi.Models.Wallets;
using BetWalletApi.Repositories.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using BetWalletApi.Services;

namespace BetWalletApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Repositories to the container.

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<ILedgerRepository, LedgerRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork,  UnitOfWork>();

            builder.Services.AddDbContext<BetWalletDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Services to the container.
            builder.Services.AddScoped<IWalletService, WalletService>();

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}