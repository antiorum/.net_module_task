using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerWeb.DTO
{
    public class UserDto : BaseDto
    {
        public UserDto(string name, long id) : base(id)
        {
            Name = name;
        }

        public string Name { get; }

        protected bool Equals(UserDto other)
        {
          return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((UserDto) obj);
        }

        public override int GetHashCode()
        {
          return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
