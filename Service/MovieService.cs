using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Service
{
	public class MovieService : IMovieService {
		private readonly ICache cache;

		public MovieService(ICache cache) {
            this.cache = cache;
		}

		public List<Movie> GetAllMovies() {
			return cache.GetAllMovies();
		}

		public Movie GetMovie(int id) {
			return cache.GetMovie(id);
		}
	}
}
