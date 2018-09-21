using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.EF {
    public class RatingMovie
    {
		public int RatingId { get; set; }
		public virtual Rating Rating { get; set; }

		public int MovieId { get; set; }
		public virtual Movie Movie { get; set; }
	}
}
