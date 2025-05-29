using CryptexApi.Enums;
using CryptexApi.Helpers;
using CryptexApi.Models.Persons;
using CryptexApi.Models.Wallets;
using CryptexApi.Models;
using CryptexApi.UnitOfWork;
using CryptexApi.Models.Identity;
using CryptexApi.Services.Interfaces;
using CryptexApi.Strings;

namespace CryptexApi.Services
{
    public class UserService : IUserService
    {
        private readonly IWalletService _walletService;

        private readonly ITicketService _ticketService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IEmailService _emailService;

        private readonly IMastodonService _mastodonService;
        public UserService( IWalletService walletService,
            ITicketService ticketService, IUnitOfWork unitOfWork, IEmailService emailService, IMastodonService mastodonService)
        {
            _walletService = walletService;
            _ticketService = ticketService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _mastodonService = mastodonService;
        }   
        public async Task<User> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository
                .GetSingleByConditionAsync(u => u.Id == id);
            return user.Data;
        }
        public async Task<User> GetByGoogleId(string googleId)
        {
            if (!string.IsNullOrEmpty(googleId))
            {
                var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.GoogleID == googleId);
                return user.Data;
            }

            return null;
        }
        public async Task<User> Insert(
            RegistrationModel registrationModel,
            Wallet wallet,
            bool IsGoogleRegistration = false
            )
        {
            if (registrationModel != null)
            {
                var parsedRole = Enum.Parse<Role>(registrationModel.Role);
                var baseUser = new User()
                {
                    GoogleID = registrationModel.GoogleID,
                    Email = registrationModel.Email,
                    Name = registrationModel.Name,
                    Surname = registrationModel.Surname,
                    PhoneNumber = registrationModel.PhoneNumber,
                    Wallet = wallet,
                    WalletId = wallet.Id,
                    Age = registrationModel.Age,
                    Country = registrationModel.Country,
                    Adress = registrationModel.Adress,
                    Role = parsedRole
                };

                var userEntity = parsedRole switch
                {
                    Role.Admin => new Admin(baseUser), 
                    Role.Support => new Support(baseUser),
                    _ => baseUser 
                };

                if (!IsGoogleRegistration)
                {
                    var hashedPassword = PasswordHasher.HashPassword(registrationModel.Password);
                    userEntity.PasswordHash = hashedPassword.hash;
                    userEntity.PasswordSalt = hashedPassword.salt;
                }

                await _unitOfWork.UserRepository.AddAsync(userEntity);
                await _unitOfWork.SaveChangesAsync();

                return userEntity;
            }

            return null;
        }

        public async Task<User> Login(LoginModel loginModel)
        {
            if (loginModel != null
                && !string.IsNullOrEmpty(loginModel.Email)
                && !string.IsNullOrEmpty(loginModel.Password))
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(x => x.Email == loginModel.Email);
                var user = result.Data;

                if (user != null && PasswordHasher.VerifyPassword(loginModel.Password, user.PasswordHash, user.PasswordSalt))
                {
                    if (!user.IsSilent)
                    {
                        await _emailService.SendEmail(user.Email, EmailStrings.WelcomeSubject,
                            EmailStrings.GetWelcomeBody(user.Name, user.Surname));
                    }
                    return user;
                }
            }

            return null;
        }

        public async Task<bool> Update(User user)
        {
            await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == user.Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<double> GetTotalWalletBalance(int id)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == id);

                if (!result.IsSuccess)
                {
                    throw new Exception($"Failed to get wallet");
                }

                var user = result.Data;
                var wallet = user.Wallet;

                if (!user.IsSilent)
                {
                    await _emailService.SendEmail(user.Email, EmailStrings.BalanceSubject,
                        EmailStrings.GetBalanceBody(user.Name, user.Surname, user.Balance));
                }

                return wallet.AmountOfCoins.Sum(coin => coin.Amount * coin.Price);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to change price. Exception {ex.Message}");
            }
        }

        public async Task BuyCoin(int id, NameOfCoin coin, double amount)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == id);
               
                if (!result.IsSuccess)
                {
                    throw new Exception($"Failed to get wallet");
                }

                var user = result.Data;
                user.Wallet = await GetMyWallet(user.Id);
                var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);
                
                if (coinInWallet == null)
                {
                    throw new Exception($"Coin {coin} not found in user's wallet");
                }

                var moneyForThisOperation = coinInWallet.Price * amount;

                if (moneyForThisOperation > user.Balance)
                {
                    throw new Exception("Balance is less than required");
                }

                user.Balance += -moneyForThisOperation;
                coinInWallet.Amount += amount;
                await _walletService.UpdateCoin(coinInWallet);
                await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == id);
                await _unitOfWork.SaveChangesAsync();

                if (!user.IsSilent)
                {
                    await _emailService.SendEmail(user.Email, EmailStrings.BuyCoinSubject,
                        EmailStrings.GetBuyCoinBody(
                            user.Name, 
                            user.Surname,
                            coin, amount,
                            user.Balance
                            )
                        );
                }
            }
            catch (Exception e)
            {
                throw new Exception($"failed to buy Coin {coin}. {e.Message}");
            }
        }
        public async Task SellCoin(int id, NameOfCoin coin, double amount)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == id);

                if (!result.IsSuccess)
                {
                    throw new Exception($"Failed to get wallet");
                }

                var user = result.Data;
                user.Wallet = await GetMyWallet(user.Id);
                var coinInWallet = user.Wallet.AmountOfCoins.FirstOrDefault(c => c.Name == coin);
                
                if (coinInWallet == null)
                {
                    throw new Exception($"Coin {coin} not found in user's wallet");
                }

                if (coinInWallet.Amount < amount)
                {
                    throw new Exception($"Amount of coin in wallet is less than you want to sell");

                }
                var moneyIncomeAfterOperation = coinInWallet.Price * amount;
                user.Balance += moneyIncomeAfterOperation;
                coinInWallet.Amount -= amount;
                await _walletService.UpdateCoin(coinInWallet);
                await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == id);
                await _unitOfWork.SaveChangesAsync();

                if (!user.IsSilent)
                {
                    await _emailService.SendEmail(user.Email, EmailStrings.SellCoinSubject,
                        EmailStrings.GetSellCoinBody(
                            user.Name,
                            user.Surname,
                            coin, amount,
                            user.Balance
                        )
                    );
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}");
            }
        }

        public async Task<Ticket> CreateTicket(int id)
        {
            try
            {
                var ticket = await _ticketService.CreateTicket(id);
                var user = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == id);

                if (!user.Data.IsSilent)
                {
                    await _emailService.SendEmail(user.Data.Email, EmailStrings.WelcomeSubject,
                        EmailStrings.GetTicketCreatedBody(user.Data.Name, user.Data.Surname, ticket.Id, ticket.Status));
                }
                
                return ticket;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task SendMessageToTicketChat(int idOfTicket, int idOfAuthorOfMessage, string valueOfMessage)
        {
            try
            {
                if (idOfAuthorOfMessage == null)
                {
                    throw new Exception("Empty author of ticket");
                }
                await _ticketService.SendMessageToTicket(idOfTicket, idOfAuthorOfMessage, valueOfMessage);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to get user {e.Message}");
            }
        }
        public async Task<List<Ticket>> GetAllMyTickets(int userId)
        {
            try
            {
                var tickets = await _unitOfWork.TicketRepository.GetListByConditionAsync(e => e.UserId == userId);
                var updatedTicketsWithChatHistory = tickets.Data.ToList();
                if (tickets == null)
                {
                    throw new Exception("Failed to get tickets");
                }

                return updatedTicketsWithChatHistory;
            }
            catch (Exception e)
            {
                throw new Exception($"Error {e.Message}");
            }
        }
        
        public async Task ConvertCurrency(
            int idOfUser, 
            NameOfCoin CoinForConvert, 
            NameOfCoin imWhichCoinConvert,
            double amountOfCoinForConvert
            )
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == idOfUser);
                if (!result.IsSuccess)
                {
                    throw new Exception($"Failed to get wallet");
                }
                var user = result.Data;
                var coinForConvert = user.Wallet.AmountOfCoins.FirstOrDefault(e => e.Name == CoinForConvert);
                var coinToConvertInto = user.Wallet.AmountOfCoins.FirstOrDefault(e => e.Name == imWhichCoinConvert);

                if (coinForConvert == null || coinToConvertInto == null)
                {
                    throw new Exception("One of the coins was not found in the wallet.");
                }

                if (coinForConvert.Amount < amountOfCoinForConvert)
                {
                    throw new Exception("Not enough coins for conversion.");
                }

                var amountAfterConversion = (coinForConvert.Price / coinToConvertInto.Price) * amountOfCoinForConvert;
                coinForConvert.Amount -= amountOfCoinForConvert;
                coinToConvertInto.Amount += amountAfterConversion;
                await _walletService.UpdateCoin(coinForConvert);
                await _walletService.UpdateCoin(coinToConvertInto);
                await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == idOfUser);
                await _unitOfWork.SaveChangesAsync();

                if (!user.IsSilent)
                {
                    await _emailService.SendEmail(user.Email, EmailStrings.ConvertCurrencySubject,
                        EmailStrings.GetConvertCurrencyBody(user.Name, user.Surname, CoinForConvert,
                            coinForConvert.Amount, imWhichCoinConvert, coinToConvertInto.Amount));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fail to convert coin: {ex.Message}");
            }
        }
        public async Task<Ticket> GetTicketById(int idOfTicket)
        {
            try
            {
                var ticket = await _ticketService.GetTicketById(idOfTicket);
                if (ticket == null)
                {
                    throw new Exception("Failed to get ticket");
                }

                return ticket;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get ticket");
            }
        }

        public async Task<Wallet> GetMyWallet(int userId)
        {
            try
            {
                var result = await _unitOfWork.UserRepository
                    .GetSingleByConditionAsync(e => e.Id == userId);
                if (!result.IsSuccess)
                {
                    throw new Exception($"Failed to get wallet");
                }

                return result.Data.Wallet;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<User> SwitchSilentMode(int userId)
        {
            var user = await _unitOfWork.UserRepository
                .GetSingleByConditionAsync(e => e.Id == userId);

            user.Data.IsSilent = !user.Data.IsSilent;

            await _unitOfWork.UserRepository.UpdateAsync(user.Data, e => e.Id == userId);
            await _unitOfWork.SaveChangesAsync();

            return user.Data;
        }

        public async Task RecomendCryptex(int userId)
        {
            var result = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.Id == userId);
            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception($"User with ID {userId} not found.");
            }

            var user = result.Data;

            var tweet = string.Format(MastodonStrings.MastodonRecommendationTemplate, user.Name, user.Surname);

            var success = await _mastodonService.PostStatusAsync(tweet);

            if (!success)
            {
                throw new Exception("Failed to post recommendation tweet.");
            }
        }

        public async Task<User> ChangeBalance(int userId, double amount)
        {
            var result = await _unitOfWork.UserRepository
                .GetSingleByConditionAsync(e => e.Id == userId);

            if (!result.IsSuccess)
            {
                throw new Exception($"Failed to get wallet");
            }

            var user = result.Data;
            user.Balance += amount;

            await _unitOfWork.UserRepository.UpdateAsync(user, e => e.Id == userId);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
    }
}
