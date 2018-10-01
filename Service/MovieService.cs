using Microsoft.AspNetCore.Http;
using Persistence;
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

		public MovieService(ICache cache, IUserService userService, IDateService dateService) {
            this.cache = cache;
            this.userService = userService;
            this.dateService = dateService;
		}

		public List<Movie> GetAllMovies() {
            var movies = cache.GetAllMovies();

            // Set favorites for authenticated user
            var username = userService.GetUserName();
            if(username != null) {
                for (int i = 0; i < movies.Count; i++) {
                    movies[i].IsFavorite = cache.IsFavorite(username, movies[i]);
                }
            }
            

			return movies;
		}
        
		public Movie GetMovie(int id) {
			return cache.GetMovie(id);
		}

        public Comment AddComment(Comment comment)
        {
            //Add the authenticated user's name
            comment.Author = userService.GetUserName();

            cache.AddComment(comment);
            return comment;
        }

        public Rating Rate(Rating rating)
        {
            //Add the authenticated user's name
            rating.Username = userService.GetUserName();

            cache.Rate(rating);
            return rating;
        }

        public void SetFavorite(bool isFavorite, int movieId) {
            var username = userService.GetUserName();
            cache.SetFavorite(isFavorite, movieId, username);
        }

        public Rating GetRating(int movieId) {
            //Get the authenticated user's name
            var username = userService.GetUserName();

            var rating = new Rating();
            var ratingValue = cache.GetRating(movieId, username);
            if (ratingValue.HasValue) rating.Value = ratingValue.Value;
            else rating.Value = 1;
            
            return rating;
        }
    }
}
