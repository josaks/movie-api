using Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Service
{
	public class MovieService : IMovieService {
		private readonly IMovieRepository dal;

		public MovieService(IMovieRepository movieDal) {
			dal = movieDal;
		}

		public List<Movie> GetAllMovies() {
			return dal.GetAllMovies();
		}

		public Movie GetMovie(int id) {
			return dal.GetMovie(id);
		}
	}
}
