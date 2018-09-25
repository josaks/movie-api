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

		public MovieService(ICache cache, IUserService userService) {
            this.cache = cache;
            this.userService = userService;
		}

		public List<Movie> GetAllMovies() {
            var movies = cache.GetAllMovies();
            for(int i = 0; i < movies.Count; i++) {
                movies[i].IsFavorite = cache.IsFavorite(userService.GetUserName(), movies[i]);
            }
			return movies;
		}

		public Movie GetMovie(int id) {
			return cache.GetMovie(id);
		}

        public void AddComment(Comment comment)
        {
            //Add the authenticated user's name
            comment.Author = userService.GetUserName();

            //Add the current date
            comment.Date = DateTime.Now;

            cache.AddComment(comment);
        }

        public void Rate(Rating rating)
        {
            //Add the authenticated user's name
            rating.Username = userService.GetUserName();

            //Add the current date
            rating.Date = DateTime.Now;

            cache.Rate(rating);
        }

        public void SetFavorite(bool isFavorite, int movieId) {
            var username = userService.GetUserName();
            cache.SetFavorite(isFavorite, movieId, username);
        }
    }
}
