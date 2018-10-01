using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.EF;
using Microsoft.EntityFrameworkCore;
using Domain = DomainModels.EF;
using View = ViewModel;

namespace Repositories {

    // Repository for retrieving data from a data store with Entity Framework.
    // This repository uses eager loading (Include()) in its calls to the db.
    public class MovieRepositoryEF : IMovieRepository {

        private readonly MovieContext DB;

        public MovieRepositoryEF(MovieContext db) {
            DB = db;
        }

        public List<View.Movie> GetAllMovies() {
            // Find all movies
            var movies = DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings)
                .Include(m => m.Comments)
                .Select(m => ConvertMovie(m)).ToList();
           
            return movies;
        }

        public bool IsFavorite(string username, View.Movie movie) {
            // Find movie
            var movies = DB.Movies
                .Include(m => m.Favorites)
                .FirstOrDefault(m => m.Id == movie.Id);

            // Check if movie exists, if so check if user has favorited it
            if (movies != null) {
                foreach (var f in movies.Favorites) {
                    if (f.Username == username) return true;
                }
            }
            return false;
        }

        public void SetFavorite(bool isFavorite, int movieId, string username) {
            // Find movie
            var movie = DB.Movies
                .Include(m => m.Favorites)
                .FirstOrDefault(m => m.Id == movieId);

            if (movie == null) return;

            if(isFavorite) {
                // Add favorite
                movie.Favorites.Add(new Favorite { MovieId = movieId, Username = username });
            }
            else {
                // Remove favorite
                var favorite = movie.Favorites.FirstOrDefault(f => f.Username == username);
                if(favorite != null) movie.Favorites.Remove(favorite);
            }
        }

        public View.Movie GetMovie(int id) {
            // Find movie
            var movie = DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings)
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == id);

            if (movie != null) return ConvertMovie(movie);
            return null;
        }

        public void AddComment(View.Comment comment) {
            // Find movie
            var movie = DB.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == comment.MovieId);

            // If the movie exists add the comment
            if (movie != null) {
                movie.Comments.Add(new Comment() {
                    Text = comment.Text,
                    Author = comment.Author,
                });
            }
            DB.SaveChanges();
        }

        public void Rate(View.Rating rating) {
            // Find movie
            var movie = DB.Movies
                .Include(m => m.Ratings)
                .FirstOrDefault(m => m.Id == rating.MovieId);

            // If the movie exists add the rating
            if (movie != null) {
                var rat = movie.Ratings.FirstOrDefault(r => r.Username == rating.Username);
                if(rat == null) {
                    movie.Ratings.Add(new Rating() {
                        Username = rating.Username,
                        MovieId = rating.MovieId,
                        RatingValue = rating.Value,
                        Date = rating.Date,
                    });
                }
                else {
                    rat.Date = rating.Date;
                    rat.RatingValue = rating.Value;
                }
            }
            DB.SaveChanges();
        }

        // Converts from EF domain model to view model
        private View.Movie ConvertMovie(Domain.Movie movie) {
            Console.WriteLine(movie);

            var viewModelMovie = new View.Movie();
            viewModelMovie.Id = movie.Id;
            viewModelMovie.Title = movie.Title;
            viewModelMovie.Year = movie.Year;
            viewModelMovie.Genres = movie.Genres.Select(g => g.GenreValue.ToString());
            viewModelMovie.AmountOfRatings = movie.Ratings.Where(r => r.MovieId == movie.Id).Count();
            viewModelMovie.Poster = movie.Poster;
            viewModelMovie.ContentRating = movie.ContentRating;
            viewModelMovie.Duration = movie.Duration;
            viewModelMovie.ReleaseDate = movie.ReleaseDate;
            viewModelMovie.OriginalTitle = movie.OriginalTitle;
            viewModelMovie.Storyline = movie.Storyline;
            viewModelMovie.Actors = movie.Actors.Select(a => a.Actor.Name);
            viewModelMovie.ImdbRating = movie.ImdbRating;
            viewModelMovie.PosterURL = movie.PosterURL;
            viewModelMovie.Comments = movie.Comments.Select(c => new View.Comment() {
                Text = c.Text,
                Author = c.Author,
                Id = c.Id,
                MovieId = c.Movie.Id,
            });

            //Can not call Average() on empty collection
            viewModelMovie.AverageRating = movie.Ratings.Count == 0 ? 0 : movie.Ratings.Average(r => r.RatingValue);

            return viewModelMovie;
        }
    }
}