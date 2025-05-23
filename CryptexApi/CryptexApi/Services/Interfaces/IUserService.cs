using CryptexApi.Models.Base;
using CryptexApi.Models.Persons;
using CryptexApi.Models.Wallets;
using CryptexApi.Models;

namespace CryptexApi.Services.Interfaces;

public interface IUserService : ISpotOperations, IAuth
{
    Task<double> GetTotalWalletBalance(int id);
    Task<Ticket> CreateTicket(int userId);
    Task<Ticket> GetTicketById(int idOfTicket);
    Task SendMessageToTicketChat(int idOfTicket, int idOfAuthorOfMessage, string valueOfMessage);
    Task<List<Ticket>> GetAllMyTickets(int userId);
    Task<Wallet> GetMyWallet(int userId);
}