using Luis;
using Microsoft.Bot.Builder.Dialogs;
using PhoneSkill.Model;

namespace PhoneSkill
{
    public class SkillConversationState : DialogState
    {
        public SkillConversationState()
        {
            Clear();
        }

        /// <summary>
        /// Gets the authentication token needed for getting the user's contact list.
        /// </summary>
        /// <value>
        /// The authentication token needed for getting the user's contact list.
        /// </value>
        public string Token { get; internal set; }

        /// <summary>
        /// Gets the source of the user's contact list.
        /// </summary>
        /// <value>
        /// The source of the user's contact list.
        /// </value>
        public ContactSource? SourceOfContacts { get; internal set; }

        /// <summary>
        /// Gets or sets the most recent LUIS result.
        /// </summary>
        /// <value>
        /// The most recent LUIS result.
        /// </value>
        public PhoneLU LuisResult { get; set; }

        /// <summary>
        /// Gets or sets the result of the contact search (if one was performed).
        /// </summary>
        /// <value>
        /// The result of the contact search (if one was performed).
        /// </value>
        public ContactSearchResult ContactResult { get; set; }

        public void Clear()
        {
            Token = string.Empty;
            SourceOfContacts = null;
            LuisResult = new PhoneLU();
            ContactResult = new ContactSearchResult();
        }
    }
}
