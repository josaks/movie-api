using System;
using System.Collections.Generic;
using ViewModel;
using System.Linq;
using DomainModels.EF;

namespace Repositories
{
    public class MovieRepositoryEF : IMovieRepository
    {

		private readonly MovieContext DB;

		public MovieRepositoryEF(MovieContext db) {
			DB = db;
		}

		public List<ViewModel.Movie> GetAllMovies() {
			return DB.Movies.Select(m => ConvertMovie(m)).ToList();
		}

		public ViewModel.Movie GetMovie(int id) {
			var movie = DB.Movies.Where(m => m.Id == id).FirstOrDefault();
			if(movie != null) return ConvertMovie(movie);
			return null;
		}

        // From domain model to view model
        private ViewModel.Movie ConvertMovie(DomainModels.EF.Movie movie) {
            Console.WriteLine(movie);

            var viewModelMovie = new ViewModel.Movie()
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genres = movie.Genres.Select(g => g.GenreValue.ToString()),
                Ratings = movie.Ratings.Select(r => r.Rating.RatingValue),
                Poster = movie.Poster,
                ContentRating = movie.ContentRating,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                AverageRating = movie.AverageRating,
                OriginalTitle = movie.OriginalTitle,
                Storyline = movie.Storyline,
                Actors = movie.Actors.Select(a => a.Actor.Name),
                ImdbRating = movie.ImdbRating,
                PosterURL = movie.PosterURL,
            };

            return viewModelMovie;
        }
	}
}