using CryptexApi.Enums;
using CryptexApi.Models.Wallets;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;

namespace CryptexApi.Services
{
    public class CoinService : ICoinService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Coin> UpdatePrice(int id)
        {
            try
            {
                var coin = await _unitOfWork.CoinRepository
                    .GetSingleByConditionAsync(coin => coin.Id == id);
                //var currentPrice = await _httpRequests.GetPriceFromBinance(coin.Name);
                //coin.Price = currentPrice;
                //await Update(coin, e => e.Id == id);
                return coin.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to change price. Exception {ex.Message}");
            }
        }

        public async Task<List<double>> GetPriceHistory(NameOfCoin coin, string periodOfTime)
        {
            try
            {
                //var coinHistory = await _httpRequests.GetHistoricalPricesFromBinance(coin, periodOfTime);
                return new List<double> { 1 };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
