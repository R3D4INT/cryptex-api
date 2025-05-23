using CryptexApi.Enums;
using CryptexApi.Models.Wallets;
using CryptexApi.Models;
using CryptexApi.UnitOfWork;
using CryptexApi.Services.Interfaces;

namespace CryptexApi.Services
{
    public class FuethersDealService : IFuethersDealService
    {
        private IUnitOfWork _unitOfWork;
        public FuethersDealService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<FuethersDeal> CreateDeal(Coin coin, TypeOfFuetersDeal typeOfFuetersDeal,
            int leverage, int userId, double stopLoss, double takeProfit, double marginValue, double amount)
        {
            try
            {
                await _unitOfWork.CoinRepository.UpdateAsync(coin, e => e.Id == coin.Id);
                var result = await _unitOfWork.CoinRepository
                    .GetSingleByConditionAsync(e => e.Id == coin.Id);
                var updatedCoin = result.Data;
                var user = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == userId);
                var FuethersDeal = new FuethersDeal();
                FuethersDeal.CoinId = updatedCoin.Id;
                FuethersDeal.EnterPrice = updatedCoin.Price;
                FuethersDeal.Leverage = leverage;
                FuethersDeal.UserId = userId;
                FuethersDeal.StopLoss = stopLoss;
                FuethersDeal.TakeProfit = takeProfit;
                FuethersDeal.TypeOfDeal = typeOfFuetersDeal;
                FuethersDeal.TypeOfMargin = TypeOfMargin.Isolate;
                FuethersDeal.MarginValue = marginValue;
                FuethersDeal.Status = Status.InProcess;
                FuethersDeal.Amount = amount;
                await _unitOfWork.FuethersDealRepository.AddAsync(FuethersDeal);
                user.Data.Balance -= marginValue;
                await _unitOfWork.UserRepository.UpdateAsync(user.Data, e => e.Id == user.Data.Id);
                await _unitOfWork.SaveChangesAsync();

                return FuethersDeal;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<FuethersDeal> CheckFuethersDeal(int dealId)
        {
            try
            {
                var result = await _unitOfWork.FuethersDealRepository
                    .GetSingleByConditionAsync(e => e.Id == dealId);
                if (!result.IsSuccess)
                {
                    throw new Exception("Failed to get deal");
                }
                var deal = result.Data;
                var coin = await _unitOfWork.CoinRepository.GetSingleByConditionAsync(e => e.Id == deal.CoinId);
                var dealIncome = (deal.EnterPrice - coin.Data.Price) / deal.Leverage * deal.Amount;
                var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(e => e.Id == deal.UserId);
                if (dealIncome < 0 && +dealIncome >= deal.MarginValue)
                {
                    deal.Status = Status.Closed;
                    deal.MarginValue = 0;
                    user.Data.Balance += deal.MarginValue;
                }
                if (coin.Data.Price <= deal.StopLoss)
                {
                    deal.Status = Status.Closed;
                    deal.MarginValue += dealIncome;
                    user.Data.Balance += deal.MarginValue;
                }
                if (coin.Data.Price >= deal.TakeProfit)
                {
                    deal.Status = Status.Closed;
                    user.Data.Balance += deal.MarginValue + dealIncome;
                }
                await _unitOfWork.UserRepository
                    .UpdateAsync(user.Data, e => e.Id == user.Data.Id);
                await _unitOfWork.FuethersDealRepository
                    .UpdateAsync(deal, e => e.Id == deal.Id);
                await _unitOfWork.SaveChangesAsync();

                return deal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<FuethersDeal> CloseDeal(int dealId)
        {
            try
            {
                var result = await _unitOfWork.FuethersDealRepository
                    .GetSingleByConditionAsync(e => e.Id == dealId);
                if (!result.IsSuccess)
                {
                    throw new Exception("Failed to get deal");
                }

                var deal = result.Data;

                deal = await CheckFuethersDeal(deal.Id);
                if (deal.Status == Status.Closed)
                {
                    return deal;
                }

                var coin = await _unitOfWork.CoinRepository
                    .GetSingleByConditionAsync(e => e.Id == deal.CoinId);
                var dealIncome = (deal.EnterPrice - coin.Data.Price) / deal.Leverage * deal.Amount;
                var user = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == deal.UserId);
                deal.MarginValue += dealIncome;
                user.Data.Balance += deal.MarginValue;
                deal.Status = Status.Closed;
                await _unitOfWork.UserRepository.UpdateAsync(user.Data, e => e.Id == user.Data.Id);
                await _unitOfWork.FuethersDealRepository.UpdateAsync(deal, e => e.Id == deal.Id);
                await _unitOfWork.SaveChangesAsync();

                return deal;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to close the Deal {e.Message}");
            }
        }

        public async Task<List<FuethersDeal>> GetAllFuethersDealsForUser(int userId)
        {
            try
            {
                var deals = await _unitOfWork.FuethersDealRepository
                    .GetListByConditionAsync(e => e.UserId == userId);
                return (List<FuethersDeal>)deals.Data;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get deals for this user");
            }
        }
    }
}
