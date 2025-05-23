using CryptexApi.Context;
using CryptexApi.Models.Wallets;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class SeedPhraseRepository(AppDbContext context) : BaseRepository<SeedPhrase>(context), ISeedPhraseRepository;