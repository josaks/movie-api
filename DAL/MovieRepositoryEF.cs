using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.EF;
using Microsoft.EntityFrameworkCore;
using Domain = DomainModels.EF;
using View = ViewModel;

namespace Repositories {
    public class MovieRepositoryEF : IMovieRepository {

        private readonly MovieContext DB;

        public MovieRepositoryEF(MovieContext db) {
            DB = db;
        }

        public List<View.Movie> GetAllMovies() {
            var mov = DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings)
                .Include(m => m.Comments)
                .Select(m => ConvertMovie(m)).ToList();

            return mov;
        }

        public bool IsFavorite(string username, View.Movie movie) {
            var mov = DB.Movies
                .Include(m => m.Favorites)
                .FirstOrDefault(m => m.Id == movie.Id);
            if (mov != null) {
                foreach (Favorite f in mov.Favorites) {
                    if (f.Username == username) return true;
                }
            }
            return false;
        }

        public void SetFavorite(bool isFavorite, int movieId, string username) {
            var movie = DB.Movies
                .Include(m => m.Favorites)
                .FirstOrDefault(m => m.Id == movieId);

            if (movie == null) return;

            if(isFavorite) {
                movie.Favorites.Add(new Favorite { MovieId = movieId, Username = username });
            }
            else {
                var favorite = movie.Favorites.FirstOrDefault(f => f.Username == username);
                if(favorite != null) movie.Favorites.Remove(favorite);
            }
        }

        public View.Movie GetMovie(int id) {
            var movie = DB.Movies
                .Include(m => m.Actors).ThenInclude(a => a.Actor)
                .Include(m => m.Genres)
                .Include(m => m.Ratings)
                .Include(m => m.Comments)
                .Where(m => m.Id == id).FirstOrDefault();
            if (movie != null) return ConvertMovie(movie);
            return null;
        }

        public void AddComment(View.Comment comment) {
            var movie = DB.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == comment.MovieId);

            if (movie != null) {
                movie.Comments.Add(new Comment() {
                    Text = comment.Text,
                    Author = comment.Author,
                });
            }
            DB.SaveChanges();
        }

        public void Rate(View.Rating rating) {
            var movie = DB.Movies
                .Include(m => m.Ratings)
                .FirstOrDefault(m => m.Id == rating.MovieId);

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

        // From domain model to view model
        private View.Movie ConvertMovie(Domain.Movie movie) {
            Console.WriteLine(movie);

            var viewModelMovie = new View.Movie();
            viewModelMovie.Id = movie.Id;
            viewModelMovie.Title = movie.Title;
            viewModelMovie.Year = movie.Year;
            viewModelMovie.Genres = movie.Genres.Select(g => g.GenreValue.ToString());
            viewModelMovie.Ratings = movie.Ratings.Select(r => r.RatingValue);
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
            viewModelMovie.Comments = movie.Comments.Select(c => new View.Comment() {
                Text = c.Text,
                Author = c.Author,
                Id = c.Id,
                MovieId = c.Movie.Id,
            });

            return viewModelMovie;
        }
    }
}