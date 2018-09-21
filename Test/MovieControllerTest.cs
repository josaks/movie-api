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
		private Mock<IMovieService> mockService;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock service that can be passed to the controller constructor
			mockService = new Mock<IMovieService>();
		}

		[TestMethod]
        public void Movies_ReturnsAListOfMovies()
        {
			//Arrange
			mockService.Setup(service => service.GetAllMovies()).Returns(Helpers.GetTestMovies());
			controller = new MovieController(mockService.Object);

            var expected = Helpers.GetTestMovies();

            //Act
            var result = controller.Movies();

			//Assert
			var model = result.Value;
            model.SequenceEqual(expected);
		}

		[TestMethod]
		public void GivenAnId_Movie_ReturnsAMovieWithCorrectId() {
			//Arrange
			int id = 1;
            string title = "Test movie";
			mockService.Setup(service => service.GetMovie(id)).Returns(Helpers.GetTestMovie(id, title));
			controller = new MovieController(mockService.Object);

			//Act
			var result = controller.Movie(id);

			//Assert
			var model = result.Value;
			Assert.AreEqual(model.Title, title);
			Assert.AreEqual(model.Id, id);
		}
	}
}
