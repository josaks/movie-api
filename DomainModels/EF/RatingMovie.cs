using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.EF {
    public class RatingMovie
    {
		public int RatingId { get; set; }
		public Rating Rating { get; set; }

		public int MovieId { get; set; }
		public Movie Movie { get; set; }
	}
}
