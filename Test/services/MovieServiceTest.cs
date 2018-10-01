using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	[TestClass]
	public class MovieServiceTest {
		private IMovieService movieService;
		private Mock<ICache> mockCache;
        private Mock<IUserService> mockUserService;
        private Mock<IDateService> mockDateService;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock repository that can be passed to the service constructor
			mockCache = new Mock<ICache>();

            //initialize mock userservice
            mockUserService = new Mock<IUserService>();

            //initialize mock dateservice
            mockDateService = new Mock<IDateService>();
        }

		[TestMethod]
		public void GetAllMovies_ReturnsAListOfMovies() {
            //Arrange
			mockCache.Setup(cache => cache.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
			movieService = new MovieService(mockCache.Object, 
                mockUserService.Object, mockDateService.Object);

            var expected = Helpers.GetTestViewMovies();

            //Act
            var result = movieService.GetAllMovies();

            //Assert
            Assert.AreEqual(result.Count, expected.Count);
            result.SequenceEqual(expected);
		}

		[TestMethod]
		public void GivenAnId_GetMovie_ReturnsAMovieWithCorrectId() {
			//Arrange
			int id = 1;
            string title = "Test movie";
			mockCache.Setup(cache => cache.GetMovie(id)).Returns(Helpers.GetTestViewMovie(id, title));
			movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

			//Act
			var result = movieService.GetMovie(id);

			//Assert
			Assert.AreEqual(result.Title, title);
			Assert.AreEqual(result.Id, id);
		}

        [TestMethod]
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
        [TestMethod]
        public void AddRating_CallsCacheAddRatingWithCorrectArguments_Once()
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

        [TestMethod]
        public void SetFavorite_CallsCacheSetFavoriteWithCorrectArguments() {
            //Arrange
            var isFavorite = true;
            var movieId = 1;
            mockUserService.Setup(s => s.GetUserName()).Returns("user");
            movieService = new MovieService(mockCache.Object,
                mockUserService.Object, mockDateService.Object);

            //Act
            movieService.SetFavorite(isFavorite, movieId);

            //Assert
            mockCache.Verify(cache => cache.SetFavorite(isFavorite, movieId, "user"), Times.Once);
        }
    }
}
