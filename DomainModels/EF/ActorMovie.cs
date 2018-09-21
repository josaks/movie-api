using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.EF {
    public class ActorMovie
    {
		public int ActorId { get; set; }
		public virtual Actor Actor { get; set; }

		public int MovieId { get; set; }
		public virtual Movie Movie { get; set; }
	}
}
