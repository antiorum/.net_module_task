using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    public class RoomRepository : BaseRepository<Room>
    {
      public RoomRepository()
      {
        Rooms testData = new Rooms();
        this.Create( testData.TestRoom1);
      }
    }
}
