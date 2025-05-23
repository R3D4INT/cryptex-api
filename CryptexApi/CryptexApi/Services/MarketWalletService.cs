using CryptexApi.Models.Wallets;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;

namespace CryptexApi.Services
{
    public class MarketWalletService : IMarketWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarketWalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateWalletForMarket()
        {
            try
            {
                var wallet = new WalletForMarket();
                var seedPhrase = new SeedPhrase();
                wallet.AmountOfCoins = new List<Coin>();
                seedPhrase.SeedPhraseValues = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
                wallet.SeedPhrase = seedPhrase;
                //foreach (NameOfCoin name in Enum.GetValues(typeof(NameOfCoin)))
                //{
                //    var coin = new Coin { Name = name, Price = await _httpRequests.GetPriceFromBinance(name), Amount = double.MaxValue, Id = Guid.NewGuid() };
                //    wallet.AmountOfCoins.Add(coin);
                //}
                await _unitOfWork.WalletForMarketRepository.AddAsync(wallet);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception("Failed To Create Market Wallet");
            }
        }
    }
}
