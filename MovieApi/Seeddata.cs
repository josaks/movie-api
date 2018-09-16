using Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ViewModel;
using DomainModels;

public static class SeedData {
	public static void Initialize(IServiceProvider serviceProvider) {
		Console.WriteLine("Seeding db...");
		var context = serviceProvider.GetRequiredService<MovieContext>();
		serviceProvider.GetService<MovieContext>().Database.Migrate();

		// Clear movie table so we can put in seed data
		context.Database.ExecuteSqlCommand("delete from Movies");

		var movie = new DomainModels.Movie() {
			Id = 0,
			Title = "Black Panther",
			Year = 2010,
			Genres = new List<ViewModel.Genre>() {
				new Genre() { Id = 1, GenreValue = Genre.GenreEnum.Action},
				new Genre() { GenreValue = Genre.GenreEnum.Adventure},
				new Genre() { GenreValue = Genre.GenreEnum.Scifi}
			},
			Ratings = new List<DomainModels.RatingMovie>(),
			Poster = "MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg",
			ContentRating = 15,
			Duration = "PT134M",
			ReleaseDate = DateTime.Parse("2018-02-14"),
			AverageRating = 0,
			OriginalTitle = "",
			Storyline = "After the events of Captain America: Civil War, King T'Challa returns home to the reclusive, technologically advanced African nation of Wakanda to serve as his country's new leader. However, T'Challa soon finds that he is challenged for the throne from factions within his own country. When two foes conspire to destroy Wakanda, the hero known as Black Panther must team up with C.I.A. agent Everett K. Ross and members of the Dora Milaje, Wakandan special forces, to prevent Wakanda from being dragged into a world war. Written by Editor",
			Actors = new List<DomainModels.ActorMovie>(),
			ImdbRating = 7,
			PosterURL = "https://images-na.ssl-images-amazon.com/images/M/MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg"
		};

		var movie2 = new DomainModels.Movie() {
			Id = 0,
			Title = "Black Panther",
			Year = 2010,
			Genres = new List<ViewModel.Genre>(),
			Ratings = new List<DomainModels.RatingMovie>(),
			Poster = "MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg",
			ContentRating = 15,
			Duration = "PT134M",
			ReleaseDate = DateTime.Parse("2018-02-14"),
			AverageRating = 0,
			OriginalTitle = "",
			Storyline = "After the events of Captain America: Civil War, King T'Challa returns home to the reclusive, technologically advanced African nation of Wakanda to serve as his country's new leader. However, T'Challa soon finds that he is challenged for the throne from factions within his own country. When two foes conspire to destroy Wakanda, the hero known as Black Panther must team up with C.I.A. agent Everett K. Ross and members of the Dora Milaje, Wakandan special forces, to prevent Wakanda from being dragged into a world war. Written by Editor",
			Actors = new List<DomainModels.ActorMovie>(),
			ImdbRating = 7,
			PosterURL = "https://images-na.ssl-images-amazon.com/images/M/MV5BMTg1MTY2MjYzNV5BMl5BanBnXkFtZTgwMTc4NTMwNDI@._V1_SY500_CR0,0,337,500_AL_.jpg"
		};
		context.Movies.Add(movie);
		//context.Movies.Add(movie2);

		//var path = @"../../resources/database.json";
		//string textFromFile = File.ReadAllText(path);
		//var movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(textFromFile);
		//foreach (Movie m in movies) {
		//	Console.WriteLine(m.Title);
		//	//context.Movies.Add(m);
		//}
		context.SaveChanges();
	}
}

//Id = movie.Id,
//				Title = movie.Title,
//				Year = movie.Year,
//				Genres = movie.Genres,
//				Ratings = movie.Ratings.Select(r => r.RatingValue).ToList(),
//				Poster = movie.Poster,
//				ContentRating = movie.ContentRating,
//				Duration = movie.Duration,
//				ReleaseDate = movie.ReleaseDate,
//				AverageRating = movie.AverageRating,
//				OriginalTitle = movie.OriginalTitle,
//				Storyline = movie.Storyline,
//				Actors = movie.Actors.Select(a => a.Name).ToList(),
//				ImdbRating = movie.ImdbRating,
//				PosterURL = movie.PosterURL