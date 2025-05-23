using CryptexApi.Models.Wallets;

namespace CryptexApi.Services.Interfaces;

public interface ISeedPhraseService
{
    Task AddBaseWords();
    Task<SeedPhrase> GetSeedPhraseBase();
}