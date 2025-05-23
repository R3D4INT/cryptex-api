﻿using CryptexApi.Models;

namespace CryptexApi.Services.Interfaces;

public interface ISupportService
{
    Task TakeTicket(int supportId, int ticketId);
    Task CloseTicket(int ticketId, int supportId);
    Task<Ticket> GetCurrentTicket(int supportId);
    Task<List<Ticket>> GetOpenTickets();
}