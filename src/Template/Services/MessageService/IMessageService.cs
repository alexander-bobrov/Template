using System.Threading.Tasks;
using Template.Services.MessageService.Models;

namespace Template.Services.MessageService
{
    public interface IMessageService
    {
        Task<Message[]> GetBasedOnAuthor(string authorPart);

        Task<Message[]> GetBasedOnRecipient(string recipientPart);

    }
}
