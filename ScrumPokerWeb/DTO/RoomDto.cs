using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Models;

namespace ScrumPokerWeb.DTO
{
    public class RoomDto : BaseDto
    {
        public RoomDto(bool @protected,  ISet<UserDto> users, UserDto owner, DeckDto deck,  long id, int timerMinutes) : base(id)
        {
            this.Protected = @protected;
            this.Users = users;
            this.Owner = owner;
            this.Deck = deck;
            this.TimerMinutes = timerMinutes;
        }

        public bool Protected { get; }
        public ISet<UserDto> Users { get; }
        public UserDto Owner { get; }
        public DeckDto Deck { get; }
        public int TimerMinutes { get; }

        protected bool Equals(RoomDto other)
        {
          return Protected == other.Protected && Equals(Owner, other.Owner) && Equals(Deck, other.Deck) && TimerMinutes == other.TimerMinutes;
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((RoomDto) obj);
        }

        public override int GetHashCode()
        {
          return HashCode.Combine(Protected, Owner, Deck, TimerMinutes);
        }
    }
}
