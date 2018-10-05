using Microsoft.AspNetCore.Mvc;
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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Test
{
    public class MovieCacheTest {
        //private MovieCache movieCache;
        //private IMemoryCache memoryCache;
        
        
        //[Fact]
        //public void WhenCacheHasMoviesEntry_GetAllMovie_ReturnsAListOfMovies() {
        //    //Arrange
        //    movieCache = new MovieCache(memoryCache);

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
        //    movieCache = new MovieCache(mockCache.Object);

        //    //Act
        //    var result = movieCache.GetMovie(expectedId);

        //    //Assert
        //    Assert.Equal(expectedTitle, result.Title);
        //    Assert.Equal(expectedId, result.Id);
        //}

    }
}
