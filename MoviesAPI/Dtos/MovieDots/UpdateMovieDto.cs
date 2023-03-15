using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos.MovieDots
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoryLine { get; set; }
        public IFormFile Poster { get; set; }
        // these two lines make EF map primry key
        public byte GenreId { get; set; }
    }
}
