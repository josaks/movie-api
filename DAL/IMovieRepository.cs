using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Repositories
{
    public interface IMovieRepository
    {
		List<Movie> GetAllMovies();
		Movie GetMovie(int id);
        Movie AddComment(Comment comment);
	}
}
