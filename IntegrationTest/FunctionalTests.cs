using Microsoft.AspNetCore.Mvc.Testing;
using MovieApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest {
    public class FunctionalTests : IClassFixture<WebApplicationFactory<Startup>> {
        public HttpClient Client { get; }

        public FunctionalTests(WebApplicationFactory<Startup> fixture) {
            Client = fixture.CreateClient();
        }

        //[Fact]
        public async Task GetAllMovies() {
            // Arrange & Act
            var response = await Client.GetAsync("/api/movies");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
