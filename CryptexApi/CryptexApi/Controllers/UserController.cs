using CryptexApi.Enums;
using CryptexApi.Services;
using CryptexApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptexApi.Controllers;

[Authorize(Roles = "User, Support, Admin")]
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/wallet/balance")]
    public async Task<IActionResult> GetTotalWalletBalance(int id)
    {
        try
        {
            var balance = await _userService.GetTotalWalletBalance(id);
            return Ok(balance);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/user/{id}/wallet
    [HttpGet("{id}/wallet")]
    public async Task<IActionResult> GetMyWallet(int id)
    {
        try
        {
            var wallet = await _userService.GetMyWallet(id);
            return Ok(wallet);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST api/user/{id}/wallet/buy
    [HttpPost("{id}/wallet/buy")]
    public async Task<IActionResult> BuyCoin(int id, [FromQuery] NameOfCoin coin, [FromQuery] double amount)
    {
        try
        {
            await _userService.BuyCoin(id, coin, amount);
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST api/user/{id}/wallet/sell
    [HttpPost("{id}/wallet/sell")]
    public async Task<IActionResult> SellCoin(int id, [FromQuery] NameOfCoin coin, [FromQuery] double amount)
    {
        try
        {
            await _userService.SellCoin(id, coin, amount);
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/wallet/convert")]
    public async Task<IActionResult> ConvertCurrency(
        int id,
        [FromQuery] NameOfCoin coinForConvert,
        [FromQuery] NameOfCoin convertToCoin,
        [FromQuery] double amount)
    {
        try
        {
            await _userService.ConvertCurrency(id, coinForConvert, convertToCoin, amount);
            var user = await _userService.GetById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPost("{id}/tickets")]
    public async Task<IActionResult> CreateTicket(int id)
    {
        try
        {
            var ticket = await _userService.CreateTicket(id);
            return Ok(ticket);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("tickets/{ticketId}/messages")]
    public async Task<IActionResult> SendMessageToTicketChat(int ticketId, [FromQuery] int authorId, [FromBody] string message)
    {
        try
        {
            if (authorId == 0)
            {
                return BadRequest(new { message = "Empty author of ticket" });
            }

            await _userService.SendMessageToTicketChat(ticketId, authorId, message);
            return Ok(new { message = "Message sent" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Failed to send message: {e.Message}" });
        }
    }

    [HttpGet("{userId}/tickets")]
    public async Task<IActionResult> GetAllMyTickets(int userId)
    {
        try
        {
            var tickets = await _userService.GetAllMyTickets(userId);
            return Ok(tickets);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Error: {e.Message}" });
        }
    }

    [HttpPatch("{userId}/silent-switch")]
    public async Task<IActionResult> SwitchSilentMode(int userId)
    {
        try
        {
            var user = await _userService.SwitchSilentMode(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Error: {e.Message}" });
        }
    }

    [HttpGet("{userId}/recommend-cryptex")]
    public async Task<IActionResult> RecommendCryptex(int userId)
    {
        try
        {
            await _userService.RecomendCryptex(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Error: {e.Message}" });
        }
    }

    [HttpPatch("{userId}/change-balance")]
    public async Task<IActionResult> ChangeUserBalance(int userId, [FromQuery] double amount)
    {
        try
        {
            var user = await _userService.ChangeBalance(userId, amount);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = $"Error: {e.Message}" });
        }
    }
}