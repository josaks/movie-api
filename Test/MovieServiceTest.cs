using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApi.Controllers;
using Persistence;
using Repositories;
using Service;
using System;
using System.Collections.Generic;
using ViewModel;

namespace Test {
	[TestClass]
	public class MovieServiceTest {
		private IMovieService service;
		private Mock<ICache> mockCache;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock repository that can be passed to the service constructor
			mockCache = new Mock<ICache>();
		}

		[TestMethod]
		public void GetAllMovies_ReturnsAListOfMovies() {
			//Arrange
			var testMovies = GetTestMovies();
			mockCache.Setup(cache => cache.GetAllMovies()).Returns(testMovies);
			service = new MovieService(mockCache.Object);

			//Act
			var result = service.GetAllMovies();

			//Assert
			Assert.AreEqual(testMovies, result);
		}

		[TestMethod]
		public void GivenAnId_GetMovie_ReturnsAMovieWithCorrectId() {
			//Arrange
			int id = 1;
			mockCache.Setup(cache => cache.GetMovie(id)).Returns(GetTestMovie(id));
			service = new MovieService(mockCache.Object);

			//Act
			var result = service.GetMovie(id);

			//Assert
			Assert.AreEqual(result.Title, "Test movie");
			Assert.AreEqual(result.Title, id);
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
