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

        //Helper method
        //Check if value exists in cache, if so return it.
        //If not, return default value
        public T CheckCache<T>(string cacheKey) {
            var foundInCache = cache.TryGetValue(cacheKey, out T cacheEntry);

            if (foundInCache) return cacheEntry;
            return default(T);
        }
    }
}