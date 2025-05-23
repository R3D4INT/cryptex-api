using CryptexApi.Enums;

namespace CryptexApi.Services.Interfaces;

public interface IBinanceRequestService
{
    Task<double> GetCoinPriceFromBinance(NameOfCoin coinName);
}