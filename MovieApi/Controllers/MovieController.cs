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

        // GET api/movie
        [HttpGet]
		[Route("Movies")]
        public ActionResult<List<Movie>> Movies()
        {
            return MovieService.GetAllMovies();
        }

        // GET api/movie/1
        [HttpGet]
		[Route("Movies/{id}")]
		public ActionResult<Movie> Movie(int id)
        {
			return MovieService.GetMovie(id);
        }
    }
}
