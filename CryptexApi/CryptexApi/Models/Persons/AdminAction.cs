using CryptexApi.Enums;
using CryptexApi.Models.Base;

namespace CryptexApi.Models.Persons;

public class AdminAction(TypeOfAdminAction typeOfAction, int idOfAdmin) : BaseEntity 
{
    public TypeOfAdminAction TypeOfAction { get; set; } = typeOfAction;
    public int IdOfAdmin { get; set; } = idOfAdmin;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}