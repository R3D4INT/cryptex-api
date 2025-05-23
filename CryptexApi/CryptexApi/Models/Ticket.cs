using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models
{
    public class Ticket : BaseEntity
    {
        public Status Status { get; set; }
        public int UserId { get; set; }
        public List<Message> ChatHistory { get;  set; }
    }
}
