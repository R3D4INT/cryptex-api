using CryptexApi.Context;
using CryptexApi.Models;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos
{
    public class RewardRepository(AppDbContext context) : BaseRepository<Reward>(context), IRewardRepository;
}
