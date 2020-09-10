using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    public class UserCardRepository : BaseRepository<UserCard>
    {
        public UserCardRepository()
        {
         var marks = new UserMarks();
         this.Save(marks.Mark1);
         this.Save(marks.Mark2);
         this.Save(marks.Mark3);
        }
    }
}