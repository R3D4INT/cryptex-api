using CryptexApi.Models.Identity;
using CryptexApi.Models.Persons;
using CryptexApi.Models.Wallets;

namespace CryptexApi.Services.Interfaces;

public interface IAuth
{
    Task<User> GetById(int id);
    Task<User> GetByGoogleId(string googleId);
    Task<User> Login(LoginModel loginModel);
    Task<User> Insert(RegistrationModel user, Wallet wallet, bool isGoogleRegistration = false);
    Task<bool> Update(User user);
}