﻿using CryptexApi.Enums;
using CryptexApi.Helpers;
using CryptexApi.Models.Wallets;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;
using System.Globalization;
using System.Text.Json;

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

        public async Task<List<double>> GetPriceHistory(NameOfCoin coin, BinanceInterval interval)
        {
            try
            {
                using HttpClient client = new HttpClient();

                var intervalStr = interval.ToIntervalString();
                var url = $"https://api.binance.com/api/v3/klines?symbol={coin}USDT&interval={intervalStr}&limit=15";

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<List<object>>>(responseBody);

                if (data == null || data.Count == 0)
                    throw new Exception("Failed to deserialize or empty Binance response");

                return data.Select(item => double.Parse(item[4].ToString(), CultureInfo.InvariantCulture)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
