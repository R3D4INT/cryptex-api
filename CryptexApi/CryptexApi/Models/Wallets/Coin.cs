using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models.Wallets;

public class Coin : BaseEntity
{
    public NameOfCoin Name { get; set; }
    public double Price { get; set; }
    public double Amount { get; set; } = 0;
    public int WalletId { get; set; }
}