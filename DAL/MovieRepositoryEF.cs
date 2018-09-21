using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.EF;
using Microsoft.EntityFrameworkCore;
using Domain = DomainModels.EF;
using View = ViewModel;

namespace Repositories
{
    public class MovieRepositoryEF : IMovieRepository
    {

		private readonly MovieContext DB;

		public MovieRepositoryEF(MovieContext db) {
			DB = db;
		}

		public List<View.Movie> GetAllMovies() {
            return DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings).ThenInclude(r => r.Rating)
                .Include(m => m.Comments)
                .Select(m => ConvertMovie(m)).ToList();
		}

		public View.Movie GetMovie(int id) {
			var movie = DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings).ThenInclude(r => r.Rating)
                .Include(m => m.Comments)
                .Where(m => m.Id == id).FirstOrDefault();
			if(movie != null) return ConvertMovie(movie);
			return null;
		}

        public View.Movie AddComment(View.Comment comment)
        {
            var movie = DB.Movies.FirstOrDefault(m => m.Id == comment.MovieId);
            if(movie != null)
            {
                movie.Comments.Add(new Comment()
                {
                    Text = comment.Text,
                    Author = comment.Author,
                });
            }
            DB.SaveChanges();

            return ConvertMovie(movie);
        }

        // From domain model to view model
        private View.Movie ConvertMovie(Domain.Movie movie) {
            Console.WriteLine(movie);

            var viewModelMovie = new View.Movie();
            viewModelMovie.Id = movie.Id;
            viewModelMovie.Title = movie.Title;
            viewModelMovie.Year = movie.Year;
            viewModelMovie.Genres = movie.Genres.Select(g => g.GenreValue.ToString());
            viewModelMovie.Ratings = movie.Ratings.Select(r => r.Rating.RatingValue);
            viewModelMovie.Poster = movie.Poster;
            viewModelMovie.ContentRating = movie.ContentRating;
            viewModelMovie.Duration = movie.Duration;
            viewModelMovie.ReleaseDate = movie.ReleaseDate;
            viewModelMovie.AverageRating = movie.AverageRating;
            viewModelMovie.OriginalTitle = movie.OriginalTitle;
            viewModelMovie.Storyline = movie.Storyline;
            viewModelMovie.Actors = movie.Actors.Select(a => a.Actor.Name);
            viewModelMovie.ImdbRating = movie.ImdbRating;
            viewModelMovie.PosterURL = movie.PosterURL;
            viewModelMovie.Comments = movie.Comments.Select(c => c.Text);

            return viewModelMovie;
        }
	}
}