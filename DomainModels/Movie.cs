﻿using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace DomainModels {
    public class Movie
    {
		public int Id { get; set; }
		
		public string Title { get; set; }

		public int Year { get; set; }

		public List<Genre> Genres { get; set; }

		public List<RatingMovie> Ratings { get; set; } = new List<RatingMovie>();

		public string Poster { get; set; }

		public int ContentRating { get; set; }

		public string Duration { get; set; }

		public DateTime ReleaseDate { get; set; }

		public int AverageRating { get; set; }

		public string OriginalTitle { get; set; }

		public string Storyline { get; set; }

		public List<ActorMovie> Actors{ get; set; } = new List<ActorMovie>();

		public int ImdbRating { get; set; }

		public string PosterURL { get; set; }
	}
}
