using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApi.Controllers;
using Service;
using System;
using System.Collections.Generic;
using ViewModel;

namespace Test
{
    [TestClass]
    public class MovieControllerTest
    {
		private MovieController controller;
		private Mock<IMovieService> mockService;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock service that can be passed to the controller constructor
			mockService = new Mock<IMovieService>();
		}

		[TestMethod]
        public void Movies_ReturnsAnActionResult_WithAListOfMovies()
        {
			//Arrange
			mockService.Setup(service => service.GetAllMovies()).Returns(GetTestMovies());
			controller = new MovieController(mockService.Object);

			//Act
			var result = controller.Movies();

			//Assert
			Assert.IsInstanceOfType(result, typeof(ActionResult<List<Movie>>));
			var model = result.Value;
			Assert.IsTrue(model.Count == 2);
		}

		[TestMethod]
		public void GivenAnId_Movie_ReturnsAnActionResult_WithAMovie() {
			//Arrange
			int id = 1;
			mockService.Setup(service => service.GetMovie(id)).Returns(GetTestMovie(id));
			controller = new MovieController(mockService.Object);

			//Act
			var result = controller.Movie(id);

			//Assert
			Assert.IsInstanceOfType(result, typeof(ActionResult<Movie>));
			var model = result.Value;
			Assert.AreEqual(model.Title, "Test movie");
			Assert.AreEqual(model.Id, id);
		}



		//Helper methods

		private Movie GetTestMovie(int id) {
			return new Movie() {
				Id = id,
				Title = "Test movie",
			};
		}

		private List<Movie> GetTestMovies() {
			var movies = new List<Movie>();
			movies.Add(new Movie() {
				Title = "Test movie 1",
				Id = 1,
			});
			movies.Add(new Movie() {
				Title = "Test movie 1",
				Id = 1,
			});
			return movies;
		}
	}
}
