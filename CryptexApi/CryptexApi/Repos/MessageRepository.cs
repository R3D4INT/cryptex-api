using CryptexApi.Context;
using CryptexApi.Models;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.Repos;

public class MessageRepository(AppDbContext context) : BaseRepository<Message>(context), IMessageRepository;
