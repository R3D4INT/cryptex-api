using CryptexApi.Context;
using CryptexApi.Models.Persons;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class AdminActionRepository(AppDbContext context) : BaseRepository<AdminAction>(context), IAdminActionRepository;
