using CryptexApi.Models.Wallets;

namespace CryptexApi.Repos.Interfaces;

public interface ICoinRepository : IBaseRepository<Coin>
{
    Task UpdatePricesFromBinance();
}