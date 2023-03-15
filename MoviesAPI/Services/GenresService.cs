using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;
        public GenresService(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task CreateGenre(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
        }

        public void DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await _context.Genres.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Genre> GetGenreById(byte id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<bool> IsValidGenreId(byte id)
        {
            return await _context.Genres.AnyAsync(x => x.Id == id);
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
        }
    }
}
