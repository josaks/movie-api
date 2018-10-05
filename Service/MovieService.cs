using Microsoft.AspNetCore.Http;
using Persistence;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Service
{
	public class MovieService : IMovieService {
		private readonly ICache cache;
        private readonly IUserService userService;
        private readonly IDateService dateService;
        private readonly IMovieRepository movieRepo;

		public MovieService(
            ICache cache, IUserService userService, IDateService dateService,
            IMovieRepository movieRepo) {

            this.cache = cache;
            this.userService = userService;
            this.dateService = dateService;
            this.movieRepo = movieRepo;
		}

		public List<Movie> GetAllMovies() {
            //Check cache first
            var movies = cache.GetAllMovies();
            //If nothing found, get from repo
            if(movies == null) {
                movies = movieRepo.GetAllMovies();
            }

            // Set favorites for authenticated user
            var username = userService.GetUserName();
            if(username != null) {
                for (int i = 0; i < movies.Count; i++) {
                    movies[i].IsFavorite = movieRepo.IsFavorite(username, movies[i]);
                }
            }
            
			return movies;
		}
        
		public Movie GetMovie(int id) {
            //Check cache first
            var movie = cache.GetMovie(id);
            //If nothing found, get from repo
            if(movie == null) {
                movie = movieRepo.GetMovie(id);
            }
            
            //Set favorite for authenticated user
            var username = userService.GetUserName();
            if(username != null) {
                movie.IsFavorite = movieRepo.IsFavorite(username, movie);
            }

            return movie;
		}

        public Comment AddComment(Comment comment)
        {
            //Add the authenticated user's name
            comment.Author = userService.GetUserName();

            //Add current date
            comment.Date = dateService.Now();

            movieRepo.AddComment(comment);
            return comment;
        }

        public Rating Rate(Rating rating)
        {
            //Add the authenticated user's name
            rating.Username = userService.GetUserName();

            //Add current date
            rating.Date = dateService.Now();

            movieRepo.Rate(rating);
            return rating;
        }

        public void SetFavorite(bool isFavorite, int movieId) {
            var username = userService.GetUserName();

            if (isFavorite) movieRepo.AddFavorite(movieId, username);
            else movieRepo.RemoveFavorite(movieId, username);
        }

        public Rating GetRating(int movieId) {
            //Get the authenticated user's name
            var username = userService.GetUserName();

            var rating = new Rating();
            var ratingValue = movieRepo.GetRating(movieId, username);
            rating.Value = (ratingValue.HasValue) ? ratingValue.Value : 1 ;
            
            return rating;
        }
    }
}
