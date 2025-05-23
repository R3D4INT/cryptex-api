using System.ComponentModel.DataAnnotations;
using CryptexApi.Enums;
using CryptexApi.Models.Wallets;

namespace CryptexApi.Models.Base
{
    public abstract class WalletBase : BaseEntity
    {
        [MaxLength(12)]
        public SeedPhrase SeedPhrase { get; set; }
        public int SeedPhraseId { get; set; }
        public List<Coin> AmountOfCoins { get; set; }
        public WalletType WalletType { get; set; }

        public WalletBase()
        {
           
        }

        public void SeedPhraseSet(SeedPhrase seedPhrase)
        {
            SeedPhrase = seedPhrase;
        }
    }
}
