using CryptexApi.Enums;
using CryptexApi.Models;
using CryptexApi.Models.Wallets;

namespace CryptexApi.Services.Interfaces;

public interface IFuethersDealService
{
    Task<FuethersDeal> CreateDeal(Coin coin, TypeOfFuetersDeal typeOfFuetersDeal,
        int leverage, int userId, double stopLoss, double takeProfit, double marginValue, double amount);
    Task<FuethersDeal> CheckFuethersDeal(int dealId);
    Task<FuethersDeal> CloseDeal(int dealId);
    Task<List<FuethersDeal>> GetAllFuethersDealsForUser(int userId);
}