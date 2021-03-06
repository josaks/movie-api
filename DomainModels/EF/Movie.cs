﻿using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace DomainModels.EF {
    public class Movie
    {
		public int Id { get; set; }
		
		public string Title { get; set; }

		public int Year { get; set; }

		public virtual List<Genre> Genres { get; set; }

		public virtual List<Rating> Ratings { get; set; }

		public string Poster { get; set; }

		public string ContentRating { get; set; }

		public string Duration { get; set; }

		public DateTime ReleaseDate { get; set; }

		public int AverageRating { get; set; }

		public string OriginalTitle { get; set; }

		public string Storyline { get; set; }

		public virtual List<ActorMovie> Actors{ get; set; }

		public double ImdbRating { get; set; }

		public string PosterURL { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Favorite> Favorites { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public GenreEnum GenreValue { get; set; }

        public enum GenreEnum
        {
            Action = 1,
            Adventure,
            Scifi,
            Animation,
            Comedy,
            Crime,
            Drama,
            Romance,
            Thriller,
            Fantasy,
            History,
            Biography,
            Musical,
        }
    }
}
