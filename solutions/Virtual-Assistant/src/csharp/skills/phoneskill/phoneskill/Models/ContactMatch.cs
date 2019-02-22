using System;
using System.Collections.Generic;

namespace PhoneSkill.Models
{
    /// <summary>
    /// A match for a contact in the user's contact list.
    /// </summary>
    public class ContactMatch : IEquatable<ContactMatch>
    {
        /// <summary>
        /// Gets or sets the matching contact from the user's contact list.
        /// </summary>
        /// <value>
        /// The matching contact from the user's contact list.
        /// </value>
        public ContactCandidate Contact { get; set; } = new ContactCandidate();

        /// <summary>
        /// Gets or sets the score for the match.
        /// </summary>
        /// <value>
        /// The score for the match. A higher score indicates a better match.
        /// </value>
        public double Score { get; set; } = 0.0;

        public override bool Equals(object obj)
        {
            return Equals(obj as ContactMatch);
        }

        public bool Equals(ContactMatch other)
        {
            return other != null &&
                   EqualityComparer<ContactCandidate>.Default.Equals(Contact, other.Contact) &&
                   Score == other.Score;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Contact, Score);
        }

        public override string ToString()
        {
            return $"ContactMatch{{Contact={Contact}, Score={Score}}}";
        }
    }
}
