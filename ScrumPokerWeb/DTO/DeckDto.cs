using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerWeb.DTO
{
    public class DeckDto : BaseDto
    {
        public DeckDto(bool isCommon, string name, ISet<CardDto> cards, long id) : base(id)
        {
            IsCommon = isCommon;
            Name = name;
            Cards = cards;
        }

        public virtual bool IsCommon { get; }
        public virtual string Name { get;  }
        public virtual ISet<CardDto> Cards { get;  }

        protected bool Equals(DeckDto other)
        {
          return IsCommon == other.IsCommon && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((DeckDto) obj);
        }

        public override int GetHashCode()
        {
          return HashCode.Combine(IsCommon, Name);
        }
    }
}
