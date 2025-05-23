using CryptexApi.Repos.Interfaces;

namespace CryptexApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAchievmentRepository AchievmentRepository { get; }
        IAdminActionRepository AdminActionRepository { get; }
        IAdminRepository AdminRepository { get; }
        ICoinRepository CoinRepository { get; }
        IFuethersDealRepository FuethersDealRepository { get; }
        IMessageRepository MessageRepository { get; }
        IRewardRepository RewardRepository { get; }
        ISeedPhraseRepository SeedPhraseRepository { get; }
        ISupportRepository SupportRepository { get; }
        ITicketRepository TicketRepository { get; }
        IUserRepository UserRepository { get; }
        IWalletForMarketRepository WalletForMarketRepository { get; }
        IWalletRepository WalletRepository { get; }
        Task SaveChangesAsync();
    }
}
