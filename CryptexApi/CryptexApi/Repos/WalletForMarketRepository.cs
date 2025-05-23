using CryptexApi.Context;
using CryptexApi.Models.Wallets;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class WalletForMarketRepository(AppDbContext context)
    : BaseRepository<WalletForMarket>(context), IWalletForMarketRepository;