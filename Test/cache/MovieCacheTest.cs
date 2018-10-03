using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
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
    public class MovieCacheTest
    {
        private MovieCache movieCache;
        private Mock<IMovieRepository> mockRepo;
        private Mock<IMemoryCache> mockCache;
        
        //public MovieCacheTest()
        //{
        //    //Initialize a mock service that can be passed to the controller constructor
        //    mockRepo = new Mock<IMovieRepository>();
        //}
        
        //[Fact]
        //public void GetAllMovie_ReturnsAListOfMovies()
        //{
        //    //Arrange
        //    mockRepo.Setup(repo => repo.GetAllMovies()).Returns(Helpers.GetTestViewMovies());
        //    mockCache = Helpers.GetMemoryCache(new List<Movie>(), false);
        //    movieCache = new MovieCache(mockCache.Object, mockRepo.Object);

        //    var expected = Helpers.GetTestViewMovies();

        //    //Act
        //    var result = movieCache.GetAllMovies();

        //    //Assert
        //    result.SequenceEqual(expected);
        //}

        //[Theory]
        //[InlineData(false, 10, "Test movie")]
        //public void GivenAnId_Movie_ReturnsAMovieWithCorrectId(
        //    bool tryGetReturnValue, int expectedId, string expectedTitle) {

        //    //Arrange
        //    var movie = Helpers.GetTestViewMovie(expectedId, expectedTitle);

        //    mockRepo.Setup(repo => repo.GetMovie(It.IsAny<int>())).Returns(movie);
        //    mockCache = Helpers.GetMemoryCache(new Movie(), false);
        //    movieCache = new MovieCache(mockCache.Object, mockRepo.Object);

        //    //Act
        //    var result = movieCache.GetMovie(expectedId);

        //    //Assert
        //    Assert.Equal(expectedTitle, result.Title);
        //    Assert.Equal(expectedId, result.Id);
        //}
        
    }
}
