using System;
using ViewModel;
using System.Collections.Generic;

namespace Service
{
    public interface IMovieService
    {
		List<Movie> GetAllMovies();
		Movie GetMovie(int id);
        void AddComment(Comment comment);
        void Rate(Rating rating);
        void SetFavorite(bool isFavorite, int movieId);
    }
}
