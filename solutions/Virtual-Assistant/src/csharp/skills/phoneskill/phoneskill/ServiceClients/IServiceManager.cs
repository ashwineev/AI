using PhoneSkill.Common;
using PhoneSkill.Models;

namespace PhoneSkill.ServiceClients
{
    public interface IServiceManager
    {
        IContactProvider GetContactProvider(string token, ContactSource source);
    }
}
