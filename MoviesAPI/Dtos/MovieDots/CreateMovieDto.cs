using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos.MovieDots
{
    public class CreateMovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoryLine { get; set; }
        // this line to recive the file from the frontEnd
        public IFormFile Poster { get; set; }
        // these two lines make EF map primry key
        public byte GenreId { get; set; }
    }
}
