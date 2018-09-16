using System;
using System.Collections.Generic;
using ViewModel;
using System.Linq;
using DomainModels;

namespace Repositories
{
    public class MovieRepository : IMovieRepository
    {

		private readonly MovieContext DB;

		public MovieRepository(MovieContext db) {
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
		private ViewModel.Movie ConvertMovie(DomainModels.Movie movie) {
			Console.WriteLine(movie);
	
			var viewModelMovie = new ViewModel.Movie();
			viewModelMovie.Id = movie.Id;
			viewModelMovie.Title = movie.Title;
			viewModelMovie.Year = movie.Year;
			viewModelMovie.Genres = movie.Genres;
			if(movie.Ratings != null) viewModelMovie.Ratings = movie.Ratings.Select(r => r.Rating.RatingValue);
			viewModelMovie.Poster = movie.Poster;
			viewModelMovie.ContentRating = movie.ContentRating;
			viewModelMovie.Duration = movie.Duration;
			viewModelMovie.ReleaseDate = movie.ReleaseDate;
			viewModelMovie.AverageRating = movie.AverageRating;
			viewModelMovie.OriginalTitle = movie.OriginalTitle;
			viewModelMovie.Storyline = movie.Storyline;
			if(movie.Actors != null) viewModelMovie.Actors = movie.Actors.Select(a => a.Actor.Name);
			viewModelMovie.ImdbRating = movie.ImdbRating;
			viewModelMovie.PosterURL = movie.PosterURL;
			
			return viewModelMovie;
		}
	}
}