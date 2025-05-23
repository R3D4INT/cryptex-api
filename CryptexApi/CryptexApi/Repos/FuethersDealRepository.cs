using CryptexApi.Context;
using CryptexApi.Models;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class FuethersDealRepository(AppDbContext context) 
    : BaseRepository<FuethersDeal>(context), IFuethersDealRepository;
