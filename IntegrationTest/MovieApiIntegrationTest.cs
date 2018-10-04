using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MovieApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Xunit;

namespace IntegrationTest {
    public class MovieApiIntegrationTest : IClassFixture<WebApplicationFactory<Startup>> {
        public HttpClient Client { get; }

        public MovieApiIntegrationTest(WebApplicationFactory<Startup> fixture) {
            Client = fixture.CreateClient();
        }

        [Fact]
        public async Task GetAllMovies_ReturnsOK() {
            //Act
            var response = await Client.GetAsync("/api/movies");

            response.EnsureSuccessStatusCode();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddComment_WithoutAuthentication_ReturnsUnauthorized() {
            //Arrange
            var comment = new Comment {
                Text = "Test comment",
                MovieId = 1,
            };
            var commentJSON = JsonConvert.SerializeObject(comment);
            
            //Act
            var response = await Client.PostAsync(
                "/api/addcomment",
                new StringContent(commentJSON));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
