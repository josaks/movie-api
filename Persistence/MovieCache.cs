using Microsoft.Extensions.Caching.Memory;
using Repositories;
using System;
using System.Collections.Generic;
using ViewModel;

namespace Persistence {
    public class MovieCache : ICache {
        private readonly IMemoryCache cache;

        public MovieCache(IMemoryCache cache) {
            this.cache = cache;
        }

        //Get all movies from cache
        public List<Movie> GetAllMovies() {
            return CheckCache<List<Movie>>("_Movies");
        }

        //Get a movie given an id from cache
        public Movie GetMovie(int id) {
            return CheckCache<Movie>($"_Movie{id}");
        }

        //Saves a movie to cache
        public void PutMovieInCache(Movie movie) {
            PutObjectInCache($"_Movie{movie.Id}", movie, TimeSpan.FromSeconds(10));
        }

        //Saves several movies to cache
        public void PutMoviesInCache(List<Movie> movies) {
            PutObjectInCache("_Movies", movies, TimeSpan.FromSeconds(10));
        }
        
        //Check if value exists in cache, if so return it.
        //If not, return default value
        private T CheckCache<T>(string cacheKey) {
            var foundInCache = cache.TryGetValue(cacheKey, out T cacheEntry);

            if (foundInCache) return cacheEntry;
            return default(T);
        }

        //Save a key and value to cache
        private void PutObjectInCache<T>(object cacheKey, T value, TimeSpan expirationTime) {
            cache.Set(cacheKey, value, expirationTime);
        }
    }
}