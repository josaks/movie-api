using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DomainModels.EF {
	public class MovieContext : DbContext {
		public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Actor> Actors { get; set; }
		public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

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

            m.Entity<Rating>().HasKey(c => new { c.MovieId, c.Username });

            m.Entity<Favorite>().HasKey(c => new { c.Username, c.MovieId });
		}
    }
}
