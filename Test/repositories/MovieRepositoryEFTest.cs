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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain = DomainModels.EF;
using View = ViewModel;
using DomainModels.EF;

namespace Test {
    [TestClass]
    public class MovieRepositoryEFTest {

        private IMovieRepository movieRepositoryEF;
        private MovieContext context;

        [TestInitialize]
        public void BeforeEach() {
            var options = new DbContextOptionsBuilder<MovieContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            context = new MovieContext(options);
        }

        [TestMethod]
        public void GetAllMovies_ReturnsAListOfMovies() {
            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "movie"));
            context.Movies.Add(Helpers.GetTestDomainMovie(2, "movie2"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movies = movieRepositoryEF.GetAllMovies();

            //Assert
            Assert.AreEqual(1, movies.ElementAt(0).Id);
            Assert.AreEqual("movie", movies.ElementAt(0).Title);
            Assert.AreEqual(2, movies.ElementAt(1).Id);
            Assert.AreEqual("movie2", movies.ElementAt(1).Title);
        }

        [DataRow(-1, null)]
        [DataRow(0, null)]
        [DataRow(3, null)]
        [DataTestMethod]
        public void GetMovie_ReturnsNull_IfItDoesntFindGivenMovie(int id, Movie expectedMovie) {
            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "movie"));
            context.Movies.Add(Helpers.GetTestDomainMovie(2, "movie2"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movie = movieRepositoryEF.GetMovie(id);

            //Assert
            Assert.AreEqual(expectedMovie, movie);
        }

        [DataRow(1, "movie")]
        [DataRow(2, "movie2")]
        [DataTestMethod]
        public void GetMovie_ReturnsAMovie_IfItFindsMatchingId(int id, string expectedTitle) {
            //Arrange
            context.Movies.Add(Helpers.GetTestDomainMovie(1, "movie"));
            context.Movies.Add(Helpers.GetTestDomainMovie(2, "movie2"));
            context.SaveChanges();
            movieRepositoryEF = new MovieRepositoryEF(context);

            //Act
            var movie = movieRepositoryEF.GetMovie(id);

            //Assert
            Assert.AreEqual(expectedTitle, movie.Title);
            Assert.AreEqual(id, movie.Id);
        }
    }
}
