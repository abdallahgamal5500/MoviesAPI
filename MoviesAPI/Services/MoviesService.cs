using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;
        public MoviesService(ApplicationDbContext context) 
        { 
            _context = context;
        }
        public async Task CreateMovie(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Movie>> GetAllMovies(byte genreId = 0)
        {
            return await _context.Movies
                // this line filters if genreId not defult value
                .Where(m => m.GenreId == genreId || genreId == 0)
                .Include(x => x.Genre)
                .OrderByDescending(x => x.Rate)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _context.Movies
               .Include(m => m.Genre)
               .SingleOrDefaultAsync(x => x.Id == id);
        }
        public void UpdateMovie(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
        }
    }
}
