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
		private IMovieService service;
		private Mock<ICache> mockCache;
        private Mock<IUserService> mockUserService;

		[TestInitialize]
		public void BeforeEach() {
			//Initialize a mock repository that can be passed to the service constructor
			mockCache = new Mock<ICache>();

            //initialize mock userservice
            mockUserService = new Mock<IUserService>();
        }

		[TestMethod]
		public void GetAllMovies_ReturnsAListOfMovies() {
            //Arrange
			mockCache.Setup(cache => cache.GetAllMovies()).Returns(Helpers.GetTestMovies());
			service = new MovieService(mockCache.Object, mockUserService.Object);

            var expected = Helpers.GetTestMovies();

            //Act
            var result = service.GetAllMovies();

            //Assert
            
            Assert.AreEqual(result.Count, expected.Count);
            result.SequenceEqual(expected);
		}

		[TestMethod]
		public void GivenAnId_GetMovie_ReturnsAMovieWithCorrectId() {
			//Arrange
			int id = 1;
            string title = "Test movie";
			mockCache.Setup(cache => cache.GetMovie(id)).Returns(Helpers.GetTestMovie(id, title));
			service = new MovieService(mockCache.Object, mockUserService.Object);

			//Act
			var result = service.GetMovie(id);

			//Assert
			Assert.AreEqual(result.Title, title);
			Assert.AreEqual(result.Id, id);
		}

        [TestMethod]
        public void AddComment_AddedComment()
        {
            //Arrange
            Comment comment = new Comment { Author = "Test", MovieId = 1, Text = "Text", Date = DateTime.MaxValue };
            mockCache = new Mock<ICache>();
            service = new MovieService(mockCache.Object, mockUserService.Object);

            //Act
            service.AddComment(comment);

            //Assert
            mockCache.Verify(cache => cache.AddComment(comment));
        }

        [TestMethod]
        public void AddRating_AddedRating()
        {
            //Arrange
            Rating rating = new Rating { Username = "Test", MovieId = 1, Value = 10, Date = DateTime.MaxValue };
            mockCache = new Mock<ICache>();
            service = new MovieService(mockCache.Object, mockUserService.Object);

            //Act
            service.Rate(rating);

            //Assert
            mockCache.Verify(cache => cache.Rate(rating));
        }
    }
}
