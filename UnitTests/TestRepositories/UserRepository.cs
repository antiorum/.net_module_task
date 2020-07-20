using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    class UserRepository : BaseRepository<User>
    {
      public UserRepository()
      {
        Users users = new Users();
        this.Create(users.JohnUser);
        this.Create(users.ValeraUser);
        this.Create(users.BorkaUser);
    }
    }
}
