using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.EF
{
    public class Favorite
    {
        public string Username { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
