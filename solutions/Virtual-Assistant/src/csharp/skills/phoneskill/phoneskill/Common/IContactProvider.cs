using System.Collections.Generic;
using PhoneSkill.Models;

namespace PhoneSkill.Common
{
    /// <summary>
    /// Provides the user's contact list.
    /// </summary>
    public interface IContactProvider
    {
        /// <summary>
        /// Get all contacts in the user's contact list.
        /// </summary>
        /// <returns>All contacts in the user's contact list.</returns>
        IList<ContactCandidate> GetContacts();
    }
}
