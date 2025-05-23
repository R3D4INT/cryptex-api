using CryptexApi.Context;
using CryptexApi.Repos.Interfaces;

namespace CryptexApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _dbContext;
        private bool disposed = false;
        public IAchievmentRepository AchievmentRepository { get; private set; }
        public IAdminActionRepository AdminActionRepository { get; private set; }
        public IAdminRepository AdminRepository { get; private set; }
        public ICoinRepository CoinRepository { get; private set; }
        public IFuethersDealRepository FuethersDealRepository { get; private set; }
        public IMessageRepository MessageRepository { get; private set; }
        public IRewardRepository RewardRepository { get; private set; }
        public ISeedPhraseRepository SeedPhraseRepository { get; private set; }
        public ISupportRepository SupportRepository { get; private set; }
        public ITicketRepository TicketRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IWalletForMarketRepository WalletForMarketRepository { get; private set; }
        public IWalletRepository WalletRepository { get; private set; }
        public UnitOfWork(AppDbContext dbContext, IAchievmentRepository achievmentRepository,
            IAdminActionRepository adminActionRepository, IAdminRepository adminRepository,
            ICoinRepository coinRepository, IFuethersDealRepository fuethersDealRepository, 
            IMessageRepository messageRepository, IRewardRepository rewardRepository,
            ISeedPhraseRepository seedPhraseRepository, ISupportRepository supportRepository,
            ITicketRepository ticketRepository, IUserRepository userRepository, 
            IWalletForMarketRepository walletForMarketRepository, IWalletRepository walletRepository)
        {
            _dbContext = dbContext;
            AchievmentRepository = achievmentRepository;
            AdminActionRepository = adminActionRepository;
            AdminRepository = adminRepository;
            CoinRepository = coinRepository;
            FuethersDealRepository = fuethersDealRepository;
            MessageRepository = messageRepository;
            RewardRepository = rewardRepository;
            SeedPhraseRepository = seedPhraseRepository;
            SupportRepository = supportRepository;
            TicketRepository = ticketRepository;
            UserRepository = userRepository;
            WalletForMarketRepository = walletForMarketRepository;
            WalletRepository = walletRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}