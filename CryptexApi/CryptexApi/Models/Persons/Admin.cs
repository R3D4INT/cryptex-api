using CryptexApi.Enums;

namespace CryptexApi.Models.Persons
{
    public class Admin : Support
    {
        public List<AdminAction> HistoryOfActions { get; set; }
        public new int Salary { get; set; } = 500;
        public Admin()
        {
            HistoryOfActions = new List<AdminAction>();
        }
        public Admin(User user)
        {
            Role = Role.Admin;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Age = user.Age;
            Country = user.Country;
            PhoneNumber = user.PhoneNumber;
            Adress = user.Adress;
            Gender = user.Gender;
            Wallet = user.Wallet;
            WalletId = user.WalletId;
            FollowersIds = user.FollowersIds;
            IsBanned = user.IsBanned;
            Balance = user.Balance;
            BalanceForCopyTrading = user.BalanceForCopyTrading;
            Id = user.Id;
            Income = user.Income;
            HistoryOfActions = new List<AdminAction>();
        }
        public Admin(User user, int experience, int salary)
        {

            Role = Role.Support;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Age = user.Age;
            Country = user.Country;
            PhoneNumber = user.PhoneNumber;
            Adress = user.Adress;
            Gender = user.Gender;
            Wallet = user.Wallet;
            WalletId = user.WalletId;
            FollowersIds = user.FollowersIds;
            IsBanned = user.IsBanned;
            Balance = user.Balance;
            BalanceForCopyTrading = user.BalanceForCopyTrading;
            Id = user.Id;
            Income = user.Income;
            Experience = experience;
            Salary = salary;
        }
    }
}
