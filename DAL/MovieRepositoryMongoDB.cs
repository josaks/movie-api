using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using View = ViewModel;
using Domain = DomainModels.MongoDB;
using System.Security.Authentication;

namespace Repositories
{
    public class MovieRepositoryMongoDB : IMovieRepository
    {

        private readonly IMongoDatabase db;
        
        public MovieRepositoryMongoDB(IMongoDatabase db)
        {
            this.db = db;
        }

        public List<View.Movie> GetAllMovies()
        {
            var domainmovies = db.GetCollection<Domain.Movie>("Movies")
                .AsQueryable()
                .ToList();
            var viewMovies = new List<View.Movie>();
            foreach(Domain.Movie dm in domainmovies)
            {
                viewMovies.Add(DomainMovieToViewMovie(dm));
            }
            return viewMovies;
        }

        public View.Movie GetMovie(int id)
        {
            var movie = db.GetCollection<Domain.Movie>("Movies")
                .AsQueryable()
                .FirstOrDefault(m => m.Id.Pid == id);
            return (movie != null) ? DomainMovieToViewMovie(movie) : null;
        }

        private static View.Movie DomainMovieToViewMovie( Domain.Movie domainMovie)
        {
            var viewMovie = new View.Movie()
            {
                Id = domainMovie.Id.Pid,
                Title = domainMovie.Title,
                Year = domainMovie.Year,
                Genres = domainMovie.Genres,
                AmountOfRatings = domainMovie.Ratings.Count,
                Poster = domainMovie.Poster,
                ContentRating = domainMovie.ContentRating,
                Duration = domainMovie.Duration,
                ReleaseDate = domainMovie.ReleaseDate,
                AverageRating = domainMovie.AverageRating,
                OriginalTitle = domainMovie.OriginalTitle,
                Storyline = domainMovie.Storyline,
                Actors = domainMovie.Actors,
                ImdbRating = domainMovie.ImdbRating,
                PosterURL = domainMovie.PosterURL,
            };

            return viewMovie;
        }

        public void AddComment(View.Comment comment)
        {
            throw new NotImplementedException();
        }

        public void Rate(View.Rating rating)
        {
            throw new NotImplementedException();
        }

        public bool IsFavorite(string username, View.Movie movie) {
            throw new NotImplementedException();
        }

        public void SetFavorite(bool isFavorite, int movieId, string username) {
            throw new NotImplementedException();
        }

        public int? GetRating(int movieId, string username) {
            throw new NotImplementedException();
        }

        public void AddFavorite(int movieId, string username) {
            throw new NotImplementedException();
        }

        public void RemoveFavorite(int movieId, string username) {
            throw new NotImplementedException();
        }
    }
}
