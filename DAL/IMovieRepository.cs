using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Repositories
{
    public interface IMovieRepository
    {
        // Get all movies from a data store
		List<Movie> GetAllMovies();

        // Given an id, get a movie from a data store
		Movie GetMovie(int id);

        // Add a comment to data store
        void AddComment(Comment comment);

        // Add a rating to data store
        void Rate(Rating rating);

        // Check if a movie is a favorite for a user
        bool IsFavorite(string username, ViewModel.Movie movie);

        // Set a movie as a favorite for a user
        void SetFavorite(bool isFavorite, int movieId, string username);

        // Given a movieId and username, get the users rating of this movie.
        // If not rated, return null
        int? GetRating(int movieId, string username);
    }
}
