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
        private Mock<IMovieRepository> mockMovieRepo;
        
		public MovieServiceTest() {
			//Initialize a mock repository that can be passed to the service constructor
			mockCache = new Mock<ICache>();

            //initialize mock userservice
            mockUserService = new Mock<IUserService>();
            //initialize mock dateservice
            mockDateService = new Mock<IDateService>();
            //initialize mock movierepo
            mockMovieRepo = new Mock<IMovieRepository>();
        }

		[Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, false)]
        [InlineData("username", true, true)]
        [InlineData("username", false, false)]
        public void GetAllMovies_ReturnsAListOfMovies_WithCorrectFavoritesSet(string username, bool repoReturnsFavorite, bool favoriteExpected) {
            //Arrange
			mockCache.Setup(cache => cache.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
            mockUserService.Setup(service => service.GetUserName()).Returns(username);
            mockMovieRepo.Setup(repo => repo.IsFavorite(username, It.IsAny<Movie>())).Returns(repoReturnsFavorite);
			movieService = new MovieService(
                mockCache.Object, 
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

            var expected = Helpers.GetTestViewMovies();


            //Act
            var result = movieService.GetAllMovies();

            //Assert
            mockCache.Verify(cache => cache.GetAllMovies(), Times.Once);
            Assert.Equal(result.Count, expected.Count);
            for(int i = 0; i < result.Count; i++) {
                Assert.Equal(favoriteExpected, result[i].IsFavorite);
            }
		}

        [Theory]
        [InlineData(null, false, false)]
        [InlineData(null, true, false)]
        [InlineData("username", true, true)]
        [InlineData("username", false, false)]
        public void GivenAnId_GetMovie_ReturnsCorrectMovie_WithCorrectFavoriteSet(string username, bool repoReturnsFavorite, bool favoriteExpected) {
			//Arrange
			int id = 1;
            string title = "Test movie";
			mockCache.Setup(cache => cache.GetMovie(id)).Returns(Helpers.GetTestViewMovie(id, title));
            mockUserService.Setup(service => service.GetUserName()).Returns(username);
            mockMovieRepo.Setup(repo => repo.IsFavorite(username, It.IsAny<Movie>())).Returns(repoReturnsFavorite);
            movieService = new MovieService(
                mockCache.Object,
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

			//Act
			var result = movieService.GetMovie(id);

            //Assert
            mockCache.Verify(cache => cache.GetMovie(id), Times.Once);
            Assert.Equal(title, result.Title);
			Assert.Equal(id, result.Id);
            Assert.Equal(favoriteExpected, result.IsFavorite);
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
            movieService = new MovieService(
                mockCache.Object,
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

            //Act
            movieService.AddComment(expectedComment);

            //Assert
            mockMovieRepo.Verify(repo => repo.AddComment(It.Is<Comment>(
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
            movieService = new MovieService(
                mockCache.Object,
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

            //Act
            movieService.Rate(expectedRating);

            //Assert
            mockMovieRepo.Verify(repo => repo.Rate(It.Is<Rating>(
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
            movieService = new MovieService(
                mockCache.Object,
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

            //Act
            movieService.SetFavorite(isFavorite, movieId);

            //Assert
            if(isFavorite) mockMovieRepo.Verify(repo => repo.AddFavorite(movieId, "user"), Times.Once);
            else mockMovieRepo.Verify(repo => repo.RemoveFavorite(movieId, "user"), Times.Once);

        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(10, 10)]
        public void GetRating_ReturnsACorrectRatingObject(int? ratingValue, int expectedValue) {
            //Arrange
            int movieId = 1;
            mockUserService.Setup(s => s.GetUserName()).Returns("user");
            mockMovieRepo.Setup(repo => repo
                .GetRating(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(ratingValue);
            movieService = new MovieService(
                mockCache.Object,
                mockUserService.Object,
                mockDateService.Object,
                mockMovieRepo.Object
            );

            //Act
            var rating = movieService.GetRating(movieId);

            //Assert
            Assert.Equal(expectedValue, rating.Value);
        }
    }
}
