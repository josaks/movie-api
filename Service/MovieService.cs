using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Service
{
	public class MovieService : IMovieService {
		private readonly IMovieRepository MovieRepo;

		public MovieService(IMovieRepository movieRepo) {
            MovieRepo = movieRepo;
		}

		public List<Movie> GetAllMovies() {
			return MovieRepo.GetAllMovies();
		}

		public Movie GetMovie(int id) {
			return MovieRepo.GetMovie(id);
		}
	}
}
