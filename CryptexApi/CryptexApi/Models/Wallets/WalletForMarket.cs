using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models.Wallets
{
    public class WalletForMarket : WalletBase
    {
        public WalletForMarket()
        {
            Id = 1;
            WalletType = WalletType.Market;
        }
    }
}
