using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using View = ViewModel;
using Domain = DomainModels.EF;

namespace Test {
    class Helpers {
        //Helper methods

        public static View.Movie GetTestViewMovie(int id, string title) {
            return new View.Movie() {
                Id = id,
                Title = title,
            };
        }

        public static Domain.Movie GetTestDomainMovie(int id, string title) {
            return new Domain.Movie() {
                Id = id,
                Title = title,
            };
        }

        public static List<View.Movie> GetTestViewMovies() {
            var movies = new List<View.Movie>();
            movies.Add(new View.Movie() {
                Title = "Test movie 1",
                Id = 1,
            });
            movies.Add(new View.Movie() {
                Title = "Test movie 1",
                Id = 1,
            });
            return movies;
        }

        public static List<Domain.Movie> GetTestDomainMovies() {
            var movies = new List<Domain.Movie>();
            movies.Add(new Domain.Movie() {
                Title = "Test movie 1",
                Id = 1,
            });
            movies.Add(new Domain.Movie() {
                Title = "Test movie 1",
                Id = 1,
            });
            return movies;
        }

        public static Mock<IMemoryCache> GetMemoryCache(object expectedValue) {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(true);
            return mockMemoryCache;
        }
    }
}
