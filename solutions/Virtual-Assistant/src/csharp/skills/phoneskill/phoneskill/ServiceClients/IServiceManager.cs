using PhoneSkill.Common;
using PhoneSkill.Model;

namespace PhoneSkill.ServiceClients
{
    public interface IServiceManager
    {
        IContactProvider GetContactProvider(string token, ContactSource source);
    }
}
