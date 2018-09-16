using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DomainModels {
	public class MovieContext : DbContext {
		public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Actor> Actors { get; set; }
		public DbSet<ActorMovie> ActorMovies { get; set; }
		public DbSet<RatingMovie> RatingMovies { get; set; }

		protected override void OnModelCreating(ModelBuilder m) {
			m.Entity<ActorMovie>().HasKey(t => new { t.ActorId, t.MovieId });
			m.Entity<ActorMovie>()
				.HasOne(t => t.Movie)
				.WithMany(t => t.Actors)
				.HasForeignKey(t => t.MovieId);
			m.Entity<ActorMovie>()
				.HasOne(t => t.Actor)
				.WithMany(t => t.ActorMovies)
				.HasForeignKey(t => t.ActorId);

			m.Entity<RatingMovie>().HasKey(t => new { t.RatingId, t.MovieId });
			m.Entity<RatingMovie>()
				.HasOne(t => t.Movie)
				.WithMany(t => t.Ratings)
				.HasForeignKey(t => t.MovieId);
			m.Entity<RatingMovie>()
				.HasOne(t => t.Rating)
				.WithMany(t => t.Movie)
				.HasForeignKey(t => t.RatingId);
			
			
			//var movie = new Repositories.Model.Movie() {
			//	Id = 1,
			//	Title = "Black Panther",
			//	Year = 2010,
			//	Genres = new List<ViewModel.Genre>(),
			//	Ratings = new List<Repositories.Model.RatingMovie>(),
			//	Poster = "MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg",
			//	ContentRating = 15,
			//	Duration = "PT134M",
			//	ReleaseDate = DateTime.Parse("2018-02-14"),
			//	AverageRating = 0,
			//	OriginalTitle = "",
			//	Storyline = "After the events of Captain America: Civil War, King T'Challa returns home to the reclusive, technologically advanced African nation of Wakanda to serve as his country's new leader. However, T'Challa soon finds that he is challenged for the throne from factions within his own country. When two foes conspire to destroy Wakanda, the hero known as Black Panther must team up with C.I.A. agent Everett K. Ross and members of the Dora Milaje, Wakandan special forces, to prevent Wakanda from being dragged into a world war. Written by Editor",
			//	Actors = new List<Repositories.Model.ActorMovie>(),
			//	ImdbRating = 7,
			//	PosterURL = "https://images-na.ssl-images-amazon.com/images/M/MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg"
			//};
			//m.Entity<Rating>().HasData(
			//	new Rating() { Id = 1, RatingValue = 10 },
			//	new Rating() { Id = 2, RatingValue = 10 },
			//	new Rating() { Id = 3, RatingValue = 10 }
			//);
			//m.Entity<Actor>().HasData(
			//	new Actor() { Id = 1, Name = "Michael Jackson" },
			//	new Actor() { Id = 2, Name = "Michael Jacks" },
			//	new Actor() { Id = 3, Name = "Michael Jac" }
			//);
			//m.Entity<Movie>().HasData(movie);

			//m.Entity<ActorMovie>().HasData(
			//	new ActorMovie() { ActorId = 1, MovieId = 1 },
			//	new ActorMovie() { ActorId = 2, MovieId = 1 },
			//	new ActorMovie() { ActorId = 3, MovieId = 1 }
			//);

			//m.Entity<RatingMovie>().HasData(
			//	new RatingMovie() { RatingId = 1, MovieId = 1 },
			//	new RatingMovie() { RatingId = 2, MovieId = 1 },
			//	new RatingMovie() { RatingId = 3, MovieId = 1 }
			//);
		}
    }
}
