using System;
using System.Collections.Generic;
using System.Text;
using DataService.Models;
using UnitTests.TestData;

namespace UnitTests.TestRepositories
{
    class DiscussionResultRepository : BaseRepository<DiscussionResult>
    {
      public DiscussionResultRepository()
      {
        DiscussionResults results = new DiscussionResults();
        this.Create(results.TestDiscussionResult1);
        this.Create(results.TestDiscussionResult2);
    }
    }
}
