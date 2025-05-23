using CryptexApi.Models.Base;

namespace CryptexApi.Models.Wallets;

public class SeedPhrase : BaseEntity
{
    public List<string> SeedPhraseValues { get; set; }
}