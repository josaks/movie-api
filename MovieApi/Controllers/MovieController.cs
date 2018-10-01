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

namespace MovieApi.Controllers {
    [Route("api/")]
    [ApiController]
    public class MovieController : ControllerBase {
        private readonly IMovieService MovieService;
        private readonly IUserService UserService;

        public MovieController(IMovieService movieService, IUserService userService) {
            MovieService = movieService;
            UserService = userService;
        }

        // GET api/movies
        // Returns every movie
        [HttpGet]
        [Route("Movies")]
        public ActionResult<List<Movie>> Movies() => MovieService.GetAllMovies();

        // GET api/movie/1
        // Given an id, returns a movie.
        // If a movie with the given id does not exist, return a 404 Not found response.
        [HttpGet]
        [Route("Movie/{id}")]
        public ActionResult<Movie> Movie(int id) {
            var movie = MovieService.GetMovie(id);
            if (movie != null) return movie;
            else return NotFound();
        }

        // POST api/addcomment
        // Saves a comment made by a user.
        // If the given comment fails validation, return a 400 Bad request response.
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [Route("AddComment")]
        public IActionResult AddComment([FromBody] Comment comment) {
            if (ModelState.IsValid) {
                var addedComment = MovieService.AddComment(comment);
                return Created("Comment added", addedComment);
            }
            else return BadRequest();
        }

        // POST api/rate
        // Saves a rating made by a user
        // If the given rating fails validation, return a 400 Bad request response.
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [Route("Rate")]
        public IActionResult Rate([FromBody] Rating rating) {
            if (ModelState.IsValid) {
                var addedRating = MovieService.Rate(rating);
                return Created("Rating saved", addedRating);
            }
            else return BadRequest();
        }

        // GET api/name
        // Returns an authenticated user's name
        [HttpGet]
        [Route("Name")]
        [Authorize(Roles = "User, Admin")]
        public ContentResult Name() => Content(UserService.GetUserName());

        // POST api/setfavorite
        // Sets a movie as a favorite for a user
        [HttpPost]
        [Route("SetFavorite")]
        [Authorize(Roles = "User, Admin")]
        public void SetFavorite([FromBody] Movie movie) {
            MovieService.SetFavorite(movie.IsFavorite, movie.Id);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        [Route("GetRating/{id}")]
        public ActionResult<Rating> GetRating(int id) {
            return MovieService.GetRating(id);
        }
    }
}
