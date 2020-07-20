using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;

namespace UnitTests.TestData
{
    public class Users
    {
        public User JohnUser { get; } = new User { Id = 1, Name = "John Dou", Rooms = new HashSet<Room>() };
        public User ValeraUser { get; } = new User { Id = 2, Name = "Valera Kotov", Rooms = new HashSet<Room>() };
        public User BorkaUser { get; } = new User { Id = 3, Name = "Bor'ka", Rooms = new HashSet<Room>()};

        public User UserForInsert { get; } = new User { Id = 4, Name = "Test", Rooms = new HashSet<Room>() };
  }
}
