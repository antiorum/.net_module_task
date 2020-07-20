using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerWeb.DTO
{
    public class BaseDto
    {
        public BaseDto(long id)
        {
            this.Id = id;
        }

        public virtual long Id { get; set; }
    }
}
