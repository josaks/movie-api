using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Microsoft.AspNetCore.Mvc;
using Service;
using ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MovieApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService MovieService;

        // Dependency injection
        public MovieController(IMovieService movieService) {
            MovieService = movieService;
        }

        // GET api/movies
        [HttpGet]
        [Route("Movies")]
        public ActionResult<List<Movie>> Movies()
        {
            return MovieService.GetAllMovies();
        }

        // GET api/movie/1
        [HttpGet]
        [Route("Movie/{id}")]
        public ActionResult<Movie> Movie(int id)
        {
            var movie = MovieService.GetMovie(id);
            if (movie != null) return movie;
            else return NotFound();
        }

        // POST api/addcomment
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [Route("AddComment")]
        public void AddComment([FromBody] Comment comment)
        {
            MovieService.AddComment(comment);
        }

        //POST api/rate
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [Route("Rate")]
        public void Rate([FromBody] Rating rating)
        {
            MovieService.Rate(rating);
        }

        [HttpGet]
        [Route("Name")]
        [Authorize(Roles = "User, Admin")]
        public ContentResult Name() => Content(User.Identity.Name);

        [HttpPost]
        [Route("SetFavorite")]
        [Authorize(Roles = "User, Admin")]
        public void SetFavorite([FromBody] Movie movie) {
            MovieService.SetFavorite(movie.IsFavorite, movie.Id);
        }
    }
}
