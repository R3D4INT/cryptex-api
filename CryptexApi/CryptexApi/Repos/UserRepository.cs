using CryptexApi.Context;
using CryptexApi.Models.Persons;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
}
