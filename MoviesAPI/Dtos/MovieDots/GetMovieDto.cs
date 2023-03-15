using MoviesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos.MovieDots
{
    public class GetMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoryLine { get; set; }
        public byte[] Poster { get; set; }
        // these two lines make EF map primry key
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
        public string GenreName { get; set; }
    }
}
