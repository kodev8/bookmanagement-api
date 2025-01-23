using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;
using System.Linq;

namespace WebApplication1.Services
{
    public class AuthorService
    {
        private readonly BookDbContext _context;

        public AuthorService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorDTOOut> AddAuthor(AuthorDTOIn authorDTO)
        {
            Author author = new Author
            {
                FirstName = authorDTO.FirstName!,
                LastName = authorDTO.LastName!,
                Biography = authorDTO.Biography
            };

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return ToDTO(author);
        }

        public async Task<AuthorDTOOut?> GetAuthor(int id)
        {
            Author? author = await _context.Authors.FindAsync(id);
            return author == null ? null : ToDTO(author);
        }

        public async Task<IEnumerable<AuthorDTOOut>> GetAllAuthors()
        {
            return await _context.Authors.Select(a => ToDTO(a)).ToListAsync();
        }

        public async Task<AuthorDTOOut?> UpdateAuthor(int id, AuthorDTOIn authorDTO)
        {   
            Author? author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            author.FirstName = authorDTO.FirstName!;
            author.LastName = authorDTO.LastName!;
            author.Biography = authorDTO.Biography;

            return ToDTO(author);
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            Author? author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        private static AuthorDTOOut ToDTO(Author author)
        {
            return new AuthorDTOOut(
                author.Id,
                author.FirstName + " " + author.LastName,
                author.Biography ?? ""
            );
        }
    }
}