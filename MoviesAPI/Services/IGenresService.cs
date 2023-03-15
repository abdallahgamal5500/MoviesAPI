using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<Genre> GetGenreById(byte id);
        Task CreateGenre(Genre genre);
        Task<bool> IsValidGenreId(byte id);
        void UpdateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
