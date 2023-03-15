using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Dtos.MovieDots;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private readonly int _maxAllowedPosterSize = 1048576;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        public MoviesController(IMoviesService moviesService, IGenresService genresService)
        {
            _moviesService = moviesService;
            _genresService = genresService;
        }
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> GetAllMovies()
        {
            // this is the first way to include Genre object in the json
            var movies = await _moviesService.GetAllMovies();

            // this is the second way to include Genre object in the json
            /*
            var movies = await _context.Movies
                .Include(x => x.Genre)
                .Select(s => new GetMovieDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Year = s.Year,
                    Rate = s.Rate,
                    StoryLine = s.StoryLine,
                    GenreId = s.GenreId,
                    GenreName = s.Genre.Name,
                    Poster = s.Poster,
                })
                .OrderByDescending(x => x.Rate).ToListAsync();
            */
            return Ok(movies);
        }

        [HttpGet("GetMovieById")]
        public async Task<IActionResult> GetMovieById([BindRequired] int id)
        {
            var movie = await _moviesService.GetMovieById(id);
            if (movie == null)
                return NotFound($"No movie was found with id: {id}");

            return Ok(movie);
        }

        [HttpGet("GetMoviesByGenreId")]
        public async Task<IActionResult> GetMoviesByGenreId([BindRequired] byte genreId)
        {
            var movies = await _moviesService.GetAllMovies(genreId);

            if (movies == null)
                return NotFound($"No movies was found with GenreId: {genreId}");

            return Ok(movies);
        }

        [HttpPost("CreateMovie")]
        // FromForm is required to take all params as a form and files
        public async Task<IActionResult> CreateMovie([FromForm] CreateMovieDto dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed poster size is 1MB!");

            // this line is important to check the genre is is correct or no
            var isValidGenreId = await _genresService.IsValidGenreId(dto.GenreId);
            if (!isValidGenreId)
                return BadRequest("Invalid genre id!");

            using var memoryStream = new MemoryStream();
            await dto.Poster.CopyToAsync(memoryStream);

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                GenreId = dto.GenreId,
                Poster = memoryStream.ToArray()
            };

            _moviesService.CreateMovie(movie);

            movie.Genre = await _genresService.GetGenreById(movie.GenreId);

            return Ok(movie);
        }

        [HttpPut("UpdateMovie")]
        public async Task<IActionResult> UpdateMovie([FromForm] UpdateMovieDto dto)
        {
            var movie = await _moviesService.GetMovieById(dto.Id);
            if (movie == null )
                return NotFound($"No movie was found with id: {dto.Id}");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed poster size is 1MB!");

            var memoryStream = new MemoryStream();
            dto.Poster.CopyToAsync(memoryStream);

            movie.Title = dto.Title;
            movie.Rate = dto.Rate;
            movie.StoryLine = dto.StoryLine;    
            movie.Poster = memoryStream.ToArray();
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Genre = await _genresService.GetGenreById(movie.GenreId);

            _moviesService.UpdateMovie(movie);
            return Ok(movie);
        }

        [HttpDelete("DeleteMovie")]
        public async Task<IActionResult> DeleteMovie([BindRequired] int id) 
        {
            var movie = await _moviesService.GetMovieById(id);
            if (movie == null)
                return NotFound($"No movie was found with id: {id}");

            _moviesService.DeleteMovie(movie);
            return Ok();
        }
    }
}
