﻿using CryptexApi.Models;

namespace CryptexApi.Services.Interfaces;

public interface ITicketService
{
    Task<Ticket> CreateTicket(int userId);
    Task SendMessageToTicket(int ticketId, int userId, string messageValue);
    Task<Ticket> GetTicketById(int ticketId);
}