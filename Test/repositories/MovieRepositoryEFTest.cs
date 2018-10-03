using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using Moq;
using MovieApi.Controllers;
using Persistence;
using Repositories;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain = DomainModels.EF;
using View = ViewModel;
using DomainModels.EF;

namespace Test {
    public class MovieRepositoryEFTest {

        private IMovieRepository movieRepositoryEF;
        private MovieContext context;
        
        public MovieRepositoryEFTest() {
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            context = new MovieContext(options);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void GetAllMovies_ReturnsAListOfMovies(int amountOfMovies) {
            //Arrange
            for(int i = 1; i <= amountOfMovies; i++) {
                context.Movies.Add(new Movie());
            }
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movies = movieRepositoryEF.GetAllMovies();

            //Assert
            Assert.Equal(amountOfMovies, movies.Count);
            Assert.IsType<List<View.Movie>>(movies);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        public void GetMovie_ReturnsNull_IfItDoesntFindGivenMovie(int id) {
            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "movie"));
            context.Movies.Add(Helpers.GetTestDomainMovie(2, "movie2"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movie = movieRepositoryEF.GetMovie(id);

            //Assert
            Assert.Null(movie);
        }

        [Theory]
        [InlineData(1, "movie")]
        [InlineData(2, "movie2")]
        public void GetMovie_ReturnsAMovie_IfItFindsMatchingId(int id, string expectedTitle) {
            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "movie"));
            context.Movies.Add(Helpers.GetTestDomainMovie(2, "movie2"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movie = movieRepositoryEF.GetMovie(id);

            //Assert
            Assert.Equal(expectedTitle, movie.Title);
            Assert.Equal(id, movie.Id);
        }

        [Theory]
        [InlineData("user", 1, true)]
        [InlineData("user", -1, false)]
        [InlineData("anotheruser", 1, false)]
        public void IsFavorite_ReturnsTheCorrectResult(
            string username, int movieId, bool expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.Favorites.Add(new Favorite { MovieId = 1, Username = "user" });
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var isFavorite = movieRepositoryEF.IsFavorite(
                username,
                new View.Movie { Id = movieId });

            //Assert
            Assert.Equal(expectedResult, isFavorite);
        }
        
        [Theory]
        [InlineData(1, "user", true)]
        [InlineData(-1, "user", false)]
        [InlineData(1, "anotheruser", false)]
        public void AddFavorite_AddsFavoriteCorrectly(
            int movieId, string username, bool expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            movieRepositoryEF.AddFavorite(1, "user");

            //Assert
            Assert.Equal(
                expectedResult,
                IsActuallyFavorite(movieId, username));
        }

        [Theory]
        [InlineData(1, "user", false)]
        [InlineData(-1, "user", false)]
        [InlineData(1, "anotheruser", true)]
        public void RemoveFavorite_RemovesFavoriteCorrectly(
            int movieId, string username, bool expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.Favorites.Add(new Favorite { MovieId = 1, Username = "user" });
            context.Favorites.Add(new Favorite { MovieId = 1, Username = "anotheruser" });
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            movieRepositoryEF.RemoveFavorite(1, "user");

            //Assert
            Assert.Equal(
                expectedResult,
                IsActuallyFavorite(movieId, username));
        }
        
        [Theory]
        [InlineData(1, true)]
        [InlineData(-1, false)]
        public void AddComment_AddsCommentCorrectly(
            int movieId, bool expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            movieRepositoryEF.AddComment(new View.Comment { Id = 1, MovieId = movieId });

            //Assert
            Assert.Equal(expectedResult, MovieHasComment(movieId, 1));
        }

        [Theory]
        [InlineData(1, "user" , true)]
        [InlineData(-1, "user", false)]
        [InlineData(1, "anotheruser", false)]
        public void Rate_AddsRatingCorrectly(
            int movieId, string username, bool expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            movieRepositoryEF.Rate(new View.Rating { MovieId = 1, Username = "user" });

            //Assert
            Assert.Equal(expectedResult, MovieHasRating(movieId, username));
        }

        [Theory]
        [InlineData(1, "user", 5)]
        [InlineData(-1, "user", null)]
        [InlineData(1, "anotheruser", null)]
        public void GetRating_GetsCorrectRating(
            int movieId, string username, int? expectedResult) {

            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "title"));
            context.Ratings.Add(new Rating { MovieId = 1, Username = "user", RatingValue = 5 });
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var rating = movieRepositoryEF.GetRating(movieId, username);

            //Assert
            Assert.Equal(expectedResult, rating);
        }


        //Helpers

        private bool MovieHasRating(int movieId, string username) {
            var movie = context.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == movieId);
            if (movie == null) return false;

            var rating = movie
                .Ratings
                .FirstOrDefault(r => r.MovieId == movieId);

            if (rating == null) return false;
            if (rating.Username != username) return false;
            return true;
        }


        private bool MovieHasComment(int movieId, int commentId) {
            var movie = context.Movies
                .Include(m => m.Comments)
                .FirstOrDefault(m => m.Id == movieId);
            if (movie == null) return false;

            var comment = movie
                .Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (comment == null) return false;
            return true;
        }

        private bool IsActuallyFavorite(int movieId, string username) {
            var favorite = context.Favorites.FirstOrDefault(f => f.MovieId == movieId && f.Username == username);
            if (favorite == null) return false;
            else return true;
        }
    }
}
