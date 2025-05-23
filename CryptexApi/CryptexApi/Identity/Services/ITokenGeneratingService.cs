using System.Security.Claims;
using CryptexApi.Models.Persons;

namespace CryptexApi.Identity.Services;

public interface ITokenGeneratingService
{
    string GenerateToken(User user);
    string GenerateToken(List<Claim> userClaims);
}