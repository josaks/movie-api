﻿using System;
using System.Collections.Generic;

namespace ViewModel
{
    public class Movie
    {
		public int Id { get; set; }

		public string Title { get; set; }

		public int Year { get; set; }

		public IEnumerable<string> Genres { get; set; }

		public IEnumerable<int> Ratings { get; set; }

		public string Poster { get; set; }

		public string ContentRating { get; set; }

		public string Duration { get; set; }

		public DateTime ReleaseDate { get; set; }

		public int AverageRating { get; set; }

		public string OriginalTitle { get; set; }

		public string Storyline { get; set; }

		public IEnumerable<string> Actors { get; set; }

		public double ImdbRating { get; set; }

		public string PosterURL { get; set; }

        public IEnumerable<string> Comments { get; set; }
	}
}
