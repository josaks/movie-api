using Microsoft.Extensions.Caching.Memory;
using Repositories;
using System;
using System.Collections.Generic;
using ViewModel;

namespace Persistence
{
    public class Cache : ICache
    {
        private readonly IMemoryCache cache;
        private readonly IMovieRepository repo;

        public Cache(IMemoryCache cache, IMovieRepository repo)
        {
            this.cache = cache;
            this.repo = repo;
        }

        public List<Movie> GetAllMovies()
        {
            return CheckCacheThenDB("_Movies", () => repo.GetAllMovies());
        }

        public Movie GetMovie(int id)
        {
            return CheckCacheThenDB("_Movie", () => repo.GetMovie(id));
        }


        //Check if database record exists in cache, if so return it.
        //If not, make a call to the database to retrieve it and store it in cache
        private T CheckCacheThenDB<T>(string cacheKey, Func<T> dbCall)
        {
            T cacheEntry;

            if (!cache.TryGetValue(cacheKey, out cacheEntry))
            {
                cacheEntry = dbCall();
                cache.Set(cacheKey, cacheEntry);
            }
            
            return cacheEntry;
        }
    }
}
