using System;
using ViewModel;
using System.Collections.Generic;

namespace Service
{
    public interface IMovieService
    {
        // Get all movies
		List<Movie> GetAllMovies();

        // Given an id, get a specific movie
		Movie GetMovie(int id);

        // Add a comment
        Comment AddComment(Comment comment);

        // Add a rating
        Rating Rate(Rating rating);

        // Set a movie as a favorite for a user
        void SetFavorite(bool isFavorite, int movieId);

        // Given a movieId, get the logged in users rating of this movie.
        // If not rated, return null
        Rating GetRating(int movieId);
    }
}
