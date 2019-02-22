using Luis;
using Microsoft.Bot.Builder.Dialogs;
using PhoneSkill.Models;

namespace PhoneSkill
{
    public class SkillConversationState : DialogState
    {
        public SkillConversationState()
        {
            Clear();
        }

        // TODO What is this? Do we need it?
        public string Token { get; internal set; }

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
            LuisResult = new PhoneLU();
            ContactResult = new ContactSearchResult();
        }
    }
}
