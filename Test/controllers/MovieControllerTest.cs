using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApi.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel;

namespace Test
{
    [TestClass]
    public class MovieControllerTest
    {
		private MovieController controller;
		private Mock<IMovieService> mockMovieService;
        private Mock<IUserService> mockUserService;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock service that can be passed to the controller constructor
			mockMovieService = new Mock<IMovieService>();
            mockUserService = new Mock<IUserService>();
		}

		[TestMethod]
        public void Movies_ReturnsAListOfMovies()
        {
			//Arrange
			mockMovieService.Setup(service => service.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
			controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            var expected = Helpers.GetTestViewMovies();

            //Act
            var result = controller.Movies();

			//Assert
			var model = result.Value;
            model.SequenceEqual(expected);
		}

		[TestMethod]
		public void GivenAnId_Movie_ReturnsAMovieWithCorrectId() {
			//Arrange
			var id = 1;
            var title = "Test movie";
			mockMovieService.Setup(service => service.GetMovie(id)).Returns(Helpers.GetTestViewMovie(id, title));
			controller = new MovieController(mockMovieService.Object, mockUserService.Object);

			//Act
			var result = controller.Movie(id);

			//Assert
			var model = result.Value;
			Assert.AreEqual(model.Title, title);
			Assert.AreEqual(model.Id, id);
		}

        [TestMethod]
        public void GivenInvalidId_Movie_Returns404NotFound() {
            // Arrange
            var id = 999;
            mockMovieService.Setup(service => service.GetMovie(id)).Returns((Movie)null);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            // Act
            var result = controller.Movie(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            
        }

        [TestMethod]
        public void GivenAComment_AddComment_PassesCommentObjectToService() {
            //Arrange
            var comment = new Comment();
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            controller.AddComment(comment);

            //Assert
            mockMovieService.Verify(service => service.AddComment(comment));
        }

        [TestMethod]
        public void GivenARating_AddRating_PassesRatingObjectToService() {
            //Arrange
            var rating = new Rating();
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            controller.Rate(rating);

            //Assert
            mockMovieService.Verify(service => service.Rate(rating));
        }

        [TestMethod]
        public void GivenUserIsAuthenticated_Name_ReturnsContentResultWithName() {
            //Arrange
            var expectedUsername = "user";
            mockUserService.Setup(service => service.GetUserName()).Returns(expectedUsername);
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            var result = controller.Name();

            //Assert
            Assert.AreEqual(expectedUsername, result.Content);
        }

        [TestMethod]
        public void GivenAMovie_SetFavorite_CallsServiceToSetFavorite() {
            //Arrange
            var movie = new Movie { IsFavorite = true, Id = 1 };
            controller = new MovieController(mockMovieService.Object, mockUserService.Object);

            //Act
            controller.SetFavorite(movie);

            //Assert
            mockMovieService.Verify(service => service.SetFavorite(true, 1));
        }
    }
}
