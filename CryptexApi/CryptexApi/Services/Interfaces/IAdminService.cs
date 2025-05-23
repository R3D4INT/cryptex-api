using CryptexApi.Models.Base;

namespace CryptexApi.Services.Interfaces;

public interface IAdminService
{
    Task BanUser(int userId, int adminId);
    Task DeleteUserAccount(int userId, int adminId);
    Task UnbanUserAccount(int userId, int adminId);
}