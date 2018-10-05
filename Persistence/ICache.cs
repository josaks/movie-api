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


        //Save to cache
        void PutMovieInCache(Movie movie);
        void PutMoviesInCache(List<Movie> movies);
    }
}
