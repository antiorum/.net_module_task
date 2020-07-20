using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerWeb.DTO
{
    public class DiscussionResultDto : BaseDto
    {
        public DiscussionResultDto(DateTime begining, DateTime ending, string theme, string resume, IDictionary<string, int?> usersMarks, long id) : base(id)
        {
            Begining = begining;
            Ending = ending;
            Theme = theme;
            Resume = resume;
            UsersMarks = usersMarks;
        }

        public virtual DateTime Begining { get; set; }
        public virtual DateTime Ending { get; set; }
        public virtual string Theme { get; set; }
        public virtual string Resume { get; set; }
        public virtual IDictionary<string, int?> UsersMarks { get; set; }
        public virtual TimeSpan Duration
        {
            get
            {
                return this.Ending - this.Begining;
            }
        }

        public virtual double? AverageMark
        {
            get
            {
                return this.UsersMarks.Values.Where(v => v != null).Average();
            }
        }

        protected bool Equals(DiscussionResultDto other)
        {
          return Begining.Equals(other.Begining) && Ending.Equals(other.Ending) && Theme == other.Theme && Resume == other.Resume;
        }

        public override bool Equals(object obj)
        {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((DiscussionResultDto) obj);
        }

        public override int GetHashCode()
        {
          return HashCode.Combine(Begining, Ending, Theme, Resume);
        }
    }
}
