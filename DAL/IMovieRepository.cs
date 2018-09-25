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
        void AddComment(Comment comment);
        void Rate(Rating rating);
        bool IsFavorite(string username, ViewModel.Movie movie);
        void SetFavorite(bool isFavorite, int movieId, string username);
    }
}
