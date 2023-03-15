using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAllMovies(byte genreId = 0);
        Task<Movie> GetMovieById(int id);
        Task CreateMovie(Movie movie);
        void UpdateMovie(Movie movie);
        void DeleteMovie(Movie movie);
    }
}
