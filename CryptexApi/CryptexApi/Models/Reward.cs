using CryptexApi.Models.Base;

namespace CryptexApi.Models
{
    public class Reward : BaseEntity
    {
        public string RewardType { get; set; }
        public int value { get; set; }
    }
}
