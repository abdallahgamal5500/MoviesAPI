using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Models;
using MoviesAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }
        [HttpGet("GetAllGenres")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genresService.GetAllGenres();
            return Ok(genres);
        }

        [HttpGet("GetGenreById")]
        public async Task<IActionResult> GetGenreById([BindRequired] byte id)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre == null)
                return NotFound($"No genre was found with id: {id}");

            return Ok(genre);
        }

        [HttpPost("CreateGenre")]
        public async Task<IActionResult> CreateGenre
            ([FromBody, BindRequired, MaxLength(100)] string genreName)
        {
            var genre = new Genre
            {
                Name = genreName,
            };
            await _genresService.CreateGenre(genre);
            // this line returns the genre row after added in db
            return Ok(genre);
        }

        [HttpPut("UpdateGenre")]
        public async Task<IActionResult> UpdateGenre([FromBody] Genre genreUpdate)
        {
            var genre = await _genresService.GetGenreById(genreUpdate.Id);
            if (genre == null)
                return NotFound($"No genre was found with id: {genreUpdate.Id}");
            
            genre.Name = genreUpdate.Name;
            _genresService.UpdateGenre(genre);
            
            return Ok(genre);
        }

        [HttpDelete("DeleteGenre")]
        public async Task<IActionResult> DeleteGenre([BindRequired] byte id)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre == null)
                return NotFound($"No genre was found with id: {id}");

            _genresService.DeleteGenre(genre);
            return Ok();
        }
    }
}
