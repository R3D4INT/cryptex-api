using CryptexApi.Context;
using CryptexApi.Models.Wallets;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos
{
    public class WalletRepository(AppDbContext context) : BaseRepository<Wallet>(context), IWalletRepository;
}
