using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.EF {
    public class Rating
    {
		public int Id { get; set; }
		public int RatingValue { get; set; }

		public List<RatingMovie> Movie { get; set; } = new List<RatingMovie>();
	}
}
