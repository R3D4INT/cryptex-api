using CryptexApi.Enums;
using CryptexApi.Models.Wallets;

namespace CryptexApi.Services.Interfaces;

public interface ICoinService
{
    Task<Coin> UpdatePrice(int id);
    Task<List<double>> GetPriceHistory(NameOfCoin coin, BinanceInterval timePeriod);
}