using CryptexApi.Context;
using CryptexApi.Models.Persons;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class SupportRepository(AppDbContext context) : BaseRepository<Support>(context), ISupportRepository;
