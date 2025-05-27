namespace CryptexApi.Services.Interfaces;

public interface IMastodonService
{
    Task<bool> PostStatusAsync(string message);
}