using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.PhoneticMatching.Matchers.ContactMatcher;
using PhoneSkill.Models;

namespace PhoneSkill.Common
{
    /// <summary>
    /// Filters the user's contact list repeatedly based on the user's input to determine the right contact and phone number to call.
    /// </summary>
    public class ContactFilter
    {
        private IContactProvider contactProvider;

        public ContactFilter(IContactProvider contactProvider)
        {
            this.contactProvider = contactProvider;
        }

        /// <summary>
        /// Filters the user's contact list repeatedly based on the user's input to determine the right contact and phone number to call.
        /// </summary>
        /// <param name="state">The current conversation state. This will be modified.</param>
        public void Filter(SkillConversationState state)
        {
            var entities = state.LuisResult.Entities;
            var entitiesForSearch = new List<InstanceData>();
            entitiesForSearch.AddRange(entities._instance.contactName);
            entitiesForSearch.AddRange(entities._instance.contactRelation);
            entitiesForSearch = SortAndRemoveOverlappingEntities(entitiesForSearch);
            var searchQuery = string.Join(" ", entitiesForSearch);

            IList<ContactCandidate> contacts;
            if (state.ContactResult.Matches.Any())
            {
                contacts = new List<ContactCandidate>();
                // TODO contacts = state.ContactResult.Matches;
            }
            else
            {
                contacts = contactProvider.GetContacts();
            }

            // TODO Adjust max number of returned contacts?
            var matcher = new EnContactMatcher<ContactCandidate>(contacts, ExtractContactFields);
            var matches = matcher.FindByName(searchQuery);

            if (!state.ContactResult.RequestedPhoneNumberType.Any())
            {
                // TODO Get requested phone number type from LUIS result.
            }

            // TODO Filter by requested phone number type.

            state.ContactResult.SearchQuery = searchQuery;
            // TODO state.ContactResult.Matches = matches;
        }

        private List<InstanceData> SortAndRemoveOverlappingEntities(List<InstanceData> entities)
        {
            // TODO implement
            return entities;
        }

        private ContactFields ExtractContactFields(ContactCandidate contact)
        {
            return new ContactFields
            {
                Name = contact.Name,
            };
        }
    }
}
