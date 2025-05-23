using CryptexApi.Enums;
using CryptexApi.Models.Base;
using CryptexApi.Models.Persons;
using CryptexApi.Services.Interfaces;
using CryptexApi.UnitOfWork;

namespace CryptexApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork) :
            base()
        {
            _unitOfWork = unitOfWork;
        }

        public async Task BanUser(int userId, int adminId)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == userId);
                var user = result.Data;
                user.IsBanned = true;
                await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == user.Id);
                var admin = await _unitOfWork.AdminRepository
                    .GetSingleByConditionAsync(e => e.Id == adminId);
                admin.Data.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.BanUser, adminId));
                await _unitOfWork.AdminRepository.UpdateAsync(admin.Data, e => e.Id == adminId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to ban user {e.Message}");
            }
        }
        public async Task UnbanUserAccount(int userId, int adminId)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == userId);
                var user = result.Data;
                user.IsBanned = false;
                await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == user.Id);
                var admin = await _unitOfWork.AdminRepository
                    .GetSingleByConditionAsync(e => e.Id == adminId);
                admin.Data.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.UnbanUser, adminId));
                await _unitOfWork.AdminRepository.UpdateAsync(admin.Data, e => e.Id == adminId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to unban user {e.Message}");
            }
        }
        public async Task DeleteUserAccount(int userId, int adminId)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteAsync(e => e.Id == userId);
                var admin = await _unitOfWork.AdminRepository
                    .GetSingleByConditionAsync(e => e.Id == adminId);
                admin.Data.HistoryOfActions.Add(new AdminAction(TypeOfAdminAction.DeleteUser, adminId));
                await _unitOfWork.AdminRepository.UpdateAsync(admin.Data, e => e.Id == adminId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to delete user {e.Message}");
            }
        }
    }
}
