using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;

namespace Test
{
    class Helpers
    {
        //Helper methods

        public static Movie GetTestMovie(int id, string title)
        {
            return new Movie()
            {
                Id = id,
                Title = title,
            };
        }

        public static List<Movie> GetTestMovies()
        {
            var movies = new List<Movie>();
            movies.Add(new Movie()
            {
                Title = "Test movie 1",
                Id = 1,
            });
            movies.Add(new Movie()
            {
                Title = "Test movie 1",
                Id = 1,
            });
            return movies;
        }

        public static Mock<IMemoryCache> GetMemoryCache(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(true);
            return mockMemoryCache;
        }
        
    }
}
