using CryptexApi.Context;
using CryptexApi.Models.Persons;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos
{
    public class AdminRepository(AppDbContext context) : BaseRepository<Admin>(context), IAdminRepository;
}

