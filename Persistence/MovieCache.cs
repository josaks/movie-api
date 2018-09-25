using Microsoft.Extensions.Caching.Memory;
using Repositories;
using System;
using System.Collections.Generic;
using ViewModel;

namespace Persistence
{
    public class MovieCache : ICache
    {
        private readonly IMemoryCache cache;
        private readonly IMovieRepository repo;

        public MovieCache(IMemoryCache cache, IMovieRepository repo)
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
            return CheckCacheThenDB($"_Movie{id}", () => repo.GetMovie(id));
        }

        public void AddComment(Comment comment)
        {
            repo.AddComment(comment);
        }

        public void Rate(Rating rating)
        {
            repo.Rate(rating);
        }

        public bool IsFavorite(string username, ViewModel.Movie movie) {
            return repo.IsFavorite(username, movie);
        }

        //Helper method
        //Check if database record exists in cache, if so return it.
        //If not, make a call to the database to retrieve it and store it in cache
        public T CheckCacheThenDB<T>(string cacheKey, Func<T> dbCall)
        {
            T cacheEntry = cache.Get<T>(cacheKey);

            if (cacheEntry != null) return cacheEntry;

            cacheEntry = dbCall();
            cache.Set(cacheKey, cacheEntry, TimeSpan.FromSeconds(10));
            return cacheEntry;
        }

        public void SetFavorite(bool isFavorite, int movieId, string username) {
            repo.SetFavorite(isFavorite, movieId, username);
        }
    }
}