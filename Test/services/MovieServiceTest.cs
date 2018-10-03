using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using MovieApi.Controllers;
using Persistence;
using Repositories;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;


namespace Test {
	public class MovieServiceTest {
		private IMovieService movieService;
		private Mock<ICache> mockCache;
        private Mock<IUserService> mockUserService;
        private Mock<IDateService> mockDateService;
        
		public MovieServiceTest() {
			//Initialize a mock repository that can be passed to the service constructor
			mockCache = new Mock<ICache>();

            //initialize mock userservice
            mockUserService = new Mock<IUserService>();

            //initialize mock dateservice
            mockDateService = new Mock<IDateService>();
        }

		[Fact]
		public void GetAllMovies_ReturnsAListOfMovies() {
            //Arrange
			mockCache.Setup(cache => cache.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
			movieService = new MovieService(mockCache.Object, 
                mockUserService.Object, mockDateService.Object);

            var expected = Helpers.GetTestViewMovies();

            //Act
            var result = movieService.GetAllMovies();

            //Assert
            Assert.Equal(result.Count, expected.Count);
            result.SequenceEqual(expected);
		}

		[Fact]
		public void GivenAnId_GetMovie_ReturnsCorrectMovie() {
			//Arrange
			int id = 1;
            string title = "Test movie";
			mockCache.Setup(cache => cache.GetMovie(id)).Returns(Helpers.GetTestViewMovie(id, title));
			movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

			//Act
			var result = movieService.GetMovie(id);

			//Assert
			Assert.Equal(title, result.Title);
			Assert.Equal(id, result.Id);
		}

        [Fact]
        public void AddComment_CallsCacheAddCommentWithCorrectArguments_Once()
        {
            //Arrange
            var author = "Test";
            var movieId = 1;
            var text = "Text";
            var date = DateTime.MaxValue;
            Comment expectedComment = new Comment {
                Author = author,
                MovieId = movieId,
                Text = text,
                Date = date
            };
            mockUserService.Setup(s => s.GetUserName()).Returns(author);
            mockDateService.Setup(s => s.Now()).Returns(date);
            movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

            //Act
            movieService.AddComment(expectedComment);

            //Assert
            mockCache.Verify(cache => cache.AddComment(It.Is<Comment>(
                comment => (
                    comment.Author == author &&
                    comment.Date == date &&
                    comment.MovieId == movieId &&
                    comment.Text == text
                ))), Times.Once);
        }


        // https://stackoverflow.com/questions/4956974/verifying-a-specific-parameter-with-moq
        [Fact]
        public void Rate_CallsCacheAddRatingWithCorrectArguments_Once()
        {
            //Arrange
            var movieId = 1;
            var value = 10;
            var date = DateTime.MaxValue;
            string username = "User";
            Rating expectedRating = new Rating {
                MovieId = movieId,
                Value = value,
                Date = new DateTime(date.Ticks),
                Username = username
            };
            mockUserService.Setup(s => s.GetUserName()).Returns(username);
            mockDateService.Setup(s => s.Now()).Returns(date);
            movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

            //Act
            movieService.Rate(expectedRating);

            //Assert
            mockCache.Verify(cache => cache.Rate(It.Is<Rating>(
                rating => (
                    rating.Date == date &&
                    rating.MovieId == movieId &&
                    rating.Username == username &&
                    rating.Value == value
                ))), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetFavorite_CallsCacheSetFavoriteWithCorrectArguments(bool isFavorite) {
            //Arrange
            var movieId = 1;
            mockUserService.Setup(s => s.GetUserName()).Returns("user");
            movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

            //Act
            movieService.SetFavorite(isFavorite, movieId);

            //Assert
            if(isFavorite) mockCache.Verify(cache => cache.AddFavorite(movieId, "user"), Times.Once);
            else mockCache.Verify(cache => cache.RemoveFavorite(movieId, "user"), Times.Once);

        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(10, 10)]
        public void GetRating_ReturnsACorrectRatingObject(int? ratingValue, int expectedValue) {
            //Arrange
            int movieId = 1;
            mockUserService.Setup(s => s.GetUserName()).Returns("user");
            mockCache.Setup(cache => cache
                .GetRating(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(ratingValue);
            movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

            //Act
            var rating = movieService.GetRating(movieId);

            //Assert
            Assert.Equal(expectedValue, rating.Value);
        }
    }
}
