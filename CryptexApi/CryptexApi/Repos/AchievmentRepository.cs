using CryptexApi.Context;
using CryptexApi.Models;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class AchievmentRepository(AppDbContext context) 
    : BaseRepository<Achievment>(context), IAchievmentRepository;
