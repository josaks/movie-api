using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Persistence
{
    public interface ICache
    {
        // Get a list of all movies from a cache.
        // If there is no entry in the cache for this, make a call to a repository instead and return the result.
        List<Movie> GetAllMovies();

        // Given an id, get a specific movie from a cache.
        // If there is no entry in the cache for this, make a call to a repository instead and return the result.
        Movie GetMovie(int id);

        // Add a comment
        void AddComment(Comment comment);

        // Add a rating
        void Rate(Rating rating);

        // Check if a movie is a favorite for a user
        bool IsFavorite(string username, Movie movie);

        // Set a movie as a favorite for a user
        void SetFavorite(bool isFavorite, int movieId, string username);
    }
}
