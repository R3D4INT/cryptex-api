using CryptexApi.Models.Persons;
using CryptexApi.Models.Wallets;
using CryptexApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptexApi.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<FuethersDeal> FuethersDeals { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SeedPhrase> SeedPhrases { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletForMarket> WalletForMarkets { get; set; }
        public DbSet<Achievment> Achievments { get; set; }
        public DbSet<Reward> Rewards { get; set; }
    }
}
