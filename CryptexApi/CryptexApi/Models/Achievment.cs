using CryptexApi.Models.Base;

namespace CryptexApi.Models
{
    public class Achievment : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAwarded { get; set; }
        public Reward Reward { get; set; }
        public bool IsShared { get; set; }
    }
}
