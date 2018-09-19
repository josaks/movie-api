using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DomainModels.MongoDB
{
    public class Movie
    {
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public int movieId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("ratings")]
        public List<int> Ratings { get; set; } = new List<int>();

        [BsonElement("poster")]
        public string Poster { get; set; }

        [BsonElement("contentRating")]
        public string ContentRating { get; set; }

        [BsonElement("duration")]
        public string Duration { get; set; }

        [BsonElement("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [BsonElement("averageRating")]
        public int AverageRating { get; set; }

        [BsonElement("originaltitle")]
        public string OriginalTitle { get; set; }

        [BsonElement("storyline")]
        public string Storyline { get; set; }

        [BsonElement("actors")]
        public List<string> Actors { get; set; } = new List<string>();

        [BsonElement("imdbRating")]
        public double ImdbRating { get; set; }

        [BsonElement("posterurl")]
        public string PosterURL { get; set; }
    }
}
