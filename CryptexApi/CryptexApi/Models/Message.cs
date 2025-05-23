using CryptexApi.Models.Base;

namespace CryptexApi.Models
{
    public class Message : BaseEntity
    {
        public string Value { get; set; }
        public int AuthorId { get; set; }
        public int TicketId { get; set; }
    }
}
