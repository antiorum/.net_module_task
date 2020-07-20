using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Models
{
    public class UserCard : BaseEntity
    {
        public virtual User User { get; set; }
        public virtual Card Card { get; set; }

        public virtual DiscussionResult DiscussionResult { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserCard card &&
                   Id == card.Id &&
                   EqualityComparer<User>.Default.Equals(User, card.User) &&
                   EqualityComparer<Card>.Default.Equals(Card, card.Card) &&
                   EqualityComparer<DiscussionResult>.Default.Equals(DiscussionResult, card.DiscussionResult);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, User, Card, DiscussionResult);
        }
    }
}
