using CryptexApi.Context;
using CryptexApi.Models;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class TicketRepository(AppDbContext context) : BaseRepository<Ticket>(context), ITicketRepository;
