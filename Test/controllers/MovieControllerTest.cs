using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApi.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ViewModel;
using Xunit;

namespace Test
{
    public class MovieControllerTest
    {
		private MovieController controller;
		private Mock<IMovieService> mockMovieService;
        private Mock<IUserService> mockUserService;
        
		public MovieControllerTest() {
			//Initialize a mock service that can be passed to the controller constructor
			mockMovieService = new Mock<IMovieService>();
            mockUserService = new Mock<IUserService>();
		}

		[Fact]
        public void Movies_ReturnsAListOfMovies()
        {
			//Arrange
			mockMovieService.Setup(service => service.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
			controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            var expected = Helpers.GetTestViewMovies();

            //Act
            var result = (OkObjectResult)controller.Movies();
            var movies = (List<Movie>)result.Value;

            //Assert
            Assert.Equal(200, result.StatusCode);
            movies.SequenceEqual(expected);
		}

		[Fact]
		public void GivenAnId_Movie_ReturnsAMovieWithCorrectId() {
			//Arrange
			var id = 1;
            var title = "Test movie";
			mockMovieService.Setup(service => service.GetMovie(id)).Returns(Helpers.GetTestViewMovie(id, title));
			controller = new MovieController(mockMovieService.Object, mockUserService.Object);

			//Act
            var result = (OkObjectResult)controller.Movie(id); ;
            var movie = (Movie)result.Value;

			//Assert
			Assert.Equal(200, result.StatusCode);
			Assert.Equal(movie.Id, id);
            Assert.Equal(movie.Title, title);
        }

        [Fact]
        public void GivenInvalidId_Movie_Returns404NotFound() {
            // Arrange
            mockMovieService.Setup(service => 
                service.GetMovie(It.IsAny<int>()))
                .Returns((Movie)null);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            // Act
            var result = (NotFoundResult)controller.Movie(1000);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GivenAValidComment_AddComment_Returns201Created() {
            //Arrange
            var comment = new Comment();
            mockMovieService.Setup(service =>
                service.AddComment(It.IsAny<Comment>())).Returns(comment);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            var result = (CreatedResult)controller.AddComment(comment);
            var resultComment = (Comment)result.Value;

            //Assert
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(comment, resultComment);
        }

        [Fact]
        public void GivenAnInvalidComment_AddComment_Returns400BadRequest() {
            //Arrange
            var comment = new Comment();
            mockMovieService.Setup(service =>
                service.AddComment(It.IsAny<Comment>())).Returns(comment);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);
            controller.ModelState.AddModelError("", "");

            //Act
            var result = (BadRequestResult)controller.AddComment(comment);

            //Assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GivenAValidRating_AddRating_Returns201Created() {
            //Arrange
            var rating = new Rating();
            mockMovieService.Setup(service =>
                service.Rate(It.IsAny<Rating>())).Returns(rating);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            var result = (CreatedResult)controller.Rate(rating);
            var resultRating = (Rating)result.Value;

            //Assert
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(rating, resultRating);
        }

        [Fact]
        public void GivenAnInvalidRating_AddRating_Returns400BadRequest() {
            //Arrange
            var rating = new Rating();
            mockMovieService.Setup(service =>
                service.Rate(It.IsAny<Rating>())).Returns(rating);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);
            controller.ModelState.AddModelError("", "");

            //Act
            var result = (BadRequestResult)controller.Rate(rating);

            //Assert
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GivenUserIsAuthenticated_Name_ReturnsContentResultWithName() {
            //Arrange
            var expectedUsername = "user";
            mockUserService.Setup(service => service.GetUserName()).Returns(expectedUsername);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            var result = controller.Name();

            //Assert
            Assert.Equal(expectedUsername, result.Content);
        }

        [Fact]
        public void GivenAMovie_SetFavorite_CallsServiceToSetFavorite() {
            //Arrange
            var movie = new Movie { IsFavorite = true, Id = 1 };
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            controller.SetFavorite(movie);

            //Assert
            mockMovieService.Verify(service => service.SetFavorite(true, 1));
        }

        [Fact]
        public void GivenAMovieId_GetRating_ReturnsARating() {
            //Arrange
            var rating = new Rating {
                MovieId = 1,
            };
            mockMovieService.Setup(service => service.GetRating(It.IsAny<int>())).Returns(rating);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            var result = (OkObjectResult)controller.GetRating(1);
            var resultRating = (Rating)result.Value;

            //Controllers
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(rating, resultRating);
        }
    }
}
