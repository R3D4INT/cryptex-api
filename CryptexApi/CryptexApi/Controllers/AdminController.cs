using CryptexApi.Models.Base;
using CryptexApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptexApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin")]
[ApiController]

public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("ban/{userId}")]
    public async Task<IActionResult> BanUser(int userId, [FromQuery] int adminId)
    {
        try
        {
            await _adminService.BanUser(userId, adminId);
            return Ok("User banned successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("unban/{userId}")]
    public async Task<IActionResult> UnbanUser(int userId, [FromQuery] int adminId)
    {
        try
        {
            await _adminService.UnbanUserAccount(userId, adminId);
            return Ok("User unbanned successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId, [FromQuery] int adminId)
    {
        try
        {
            await _adminService.DeleteUserAccount(userId, adminId);
            return Ok("User deleted successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}