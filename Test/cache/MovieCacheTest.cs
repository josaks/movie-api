using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApi.Controllers;
using Persistence;
using Repositories;
using Service;
using System;
using System.Collections.Generic;
using ViewModel;
using System.Linq;

namespace Test
{
    [TestClass]
    public class MovieCacheTest
    {
        private MovieCache movieCache;
        private Mock<IMovieRepository> mockRepo;
        private Mock<IMemoryCache> mockCache;

        [TestInitialize]
        public void BeforeEach()
        {
            //Initialize a mock service that can be passed to the controller constructor
            mockRepo = new Mock<IMovieRepository>();
        }
        
        [TestMethod]
        public void GetAllMovie_ReturnsAListOfMovies()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
            mockCache = Helpers.GetMemoryCache(new List<Movie>());
            movieCache = new MovieCache(mockCache.Object, mockRepo.Object);

            var expected = Helpers.GetTestViewMovies();

            //Act
            var result = movieCache.GetAllMovies();

            //Assert
            result.SequenceEqual(expected);
        }

        //[TestMethod]
        //public void GivenAnId_Movie_ReturnsAMovieWithCorrectId()
        //{
        //    //Arrange
        //    var id = 10;
        //    var title = "Test movie";
        //    var movie = Helpers.GetTestViewMovie(id, title);

        //    mockRepo.Setup(repo => repo.GetMovie(It.IsAny<int>())).Returns(movie);
        //    mockCache = Helpers.GetMemoryCache(new Movie());
        //    movieCache = new MovieCache(mockCache.Object, mockRepo.Object);

        //    //Act
        //    var result = movieCache.GetMovie(id);

        //    //Assert
        //    Assert.AreEqual(title, result.Title);
        //    Assert.AreEqual(id, result.Id);
        //}
    }
}
