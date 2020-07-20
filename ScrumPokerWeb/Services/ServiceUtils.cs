using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerWeb.Services
{
    public static class ServiceUtils
    {
        public static IEnumerable<long> ParseIds(string source)
        {
            List<long> result = new List<long>();
            string[] ids = source.Split(", ");
            foreach(string id in ids)
            {
                result.Add(long.Parse(id));
            }
            return result;
        }
    }
}
