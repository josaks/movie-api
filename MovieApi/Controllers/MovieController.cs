using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Microsoft.AspNetCore.Mvc;
using Service;
using ViewModel;

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
        [Route("AddComment")]
        public ActionResult<Movie> AddComment([FromBody] Comment comment)
        {
            var movie = MovieService.AddComment(comment);
            if (movie != null) return movie;
            else return NotFound();
        }
    }
}
