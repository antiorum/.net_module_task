using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService;
using DataService.Models;

namespace ScrumPokerWeb.DTO
{
    public class CardDto : BaseDto
    {
        public CardDto(bool isCommon, string name, int? value, string image, long id) : base(id)
        {
            IsCommon = isCommon;
            Name = name;
            Value = value;
            Image = image;
        }

        public virtual bool IsCommon { get; set; }
        public virtual string Name { get; set; }
        public virtual int? Value { get; set; }
        public virtual string Image { get; set; }

        protected bool Equals(CardDto other)
        {
          return IsCommon == other.IsCommon && Name == other.Name && Value == other.Value && Image == other.Image;
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((CardDto) obj);
        }

        public override int GetHashCode()
        {
          return HashCode.Combine(IsCommon, Name, Value, Image);
        }
    }
}
