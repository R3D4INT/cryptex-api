using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models.Wallets
{
    public class Wallet : WalletBase
    {
        public Wallet()
        {
            WalletType = WalletType.User;
        }
    }
}
