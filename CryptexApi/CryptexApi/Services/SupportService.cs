﻿using CryptexApi.Enums;
using CryptexApi.Models;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;

namespace CryptexApi.Services
{
    public class SupportService : ISupportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;
        public SupportService(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }
        
        public async Task TakeTicket(int supportId, int ticketId)
        {
            try
            {
                var result = await _unitOfWork.TicketRepository
                    .GetSingleByConditionAsync(e => e.Id == ticketId);
                if (!result.IsSuccess)
                {
                    throw new Exception("Ticket is null");
                }

                var ticket = result.Data;
                var support = await _unitOfWork.SupportRepository
                    .GetSingleByConditionAsync(e => e.Id == supportId);

                if (support == null)
                {
                    throw new Exception("Support is null");
                }

                ticket.Status = Status.InProcess;
                await _unitOfWork.TicketRepository.UpdateAsync(ticket, e => e.Id == ticket.Id);
                support.Data.TicketInProgressId = ticket.Id;
                await _unitOfWork.SupportRepository.UpdateAsync(support.Data, e => e.Id == support.Data.Id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to take ticket {e.Message}");
            }
        }
        public async Task CloseTicket(int ticketId, int supportId)
        {
            try
            {
                var result = await _unitOfWork.TicketRepository
                    .GetSingleByConditionAsync(e => e.Id == ticketId);

                if (!result.IsSuccess)
                {
                    throw new Exception("Ticket is null");
                }

                var ticket = result.Data;
                ticket.Status = Status.Closed;
                await _unitOfWork.TicketRepository.UpdateAsync(ticket, e => e.Id == ticket.Id);
                var support = await _unitOfWork.SupportRepository
                    .GetSingleByConditionAsync(e => e.Id == supportId);
                if (support == null)
                {
                    throw new Exception("Failed to get support");
                }

                support.Data.TicketInProgress = null;
                support.Data.TicketInProgressId = null;
                support.Data.Experience++;
                await _unitOfWork.SupportRepository
                    .UpdateAsync(support.Data, e => e.Id == support.Data.Id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to close ticket {e.Message}");
            }
        }
        public async Task<List<Ticket>> GetOpenTickets()
        {
            try
            {
                var tickets = await _unitOfWork.TicketRepository.GetListByConditionAsync(e => e.Status == Status.Open);
                var ticketsList = tickets.Data.ToList();
                for (var i = 0; i < ticketsList.Count(); i++)
                {
                    ticketsList[i].ChatHistory = await _messageService.GetChatHistoryOfTicket(ticketsList[i].Id);
                }
                return (List<Ticket>)tickets.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to get open tickets {e.Message}");
            }
        }
    }
}
