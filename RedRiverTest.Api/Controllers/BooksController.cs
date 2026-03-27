using RedRiverTest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiverTest.Api.Data;

namespace RedRiverTest.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
        private readonly AppDbContext _db;

		public BooksController(AppDbContext db)
		{
			_db = db;
		}

		// Get: api/books (Hämta alla)
		[HttpGet]
       public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
		{
          var books = await _db.Books
				.AsNoTracking()
				.OrderBy(b => b.Id)
				.ToListAsync();
			return Ok(books);
		}

		// Get: api/books/{id} (Hämta en)
		[HttpGet("{id:int}")]
       public async Task<ActionResult<Book>> GetBook(int id)
		{
          var book = await _db.Books
				.AsNoTracking()
				.FirstOrDefaultAsync(b => b.Id == id);
			if (book is null)
			{
				return NotFound();
			}

			return Ok(book);
		}

		// POST: api/books (lägg till)
		[HttpPost]
     public async Task<ActionResult<Book>> CreateBook(Book book)
		{
           book.Id = 0;
         _db.Books.Add(book);
			await _db.SaveChangesAsync();
			return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
		}

		// PUT: api/books/{id} (uppdatera)
		[HttpPut("{id:int}")]
     public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
		{
          var existing = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
			if (existing is null)
			{
				return NotFound();
			}

			existing.Title = book.Title;
			existing.Author = book.Author;
			existing.PublishedDate = book.PublishedDate;
			await _db.SaveChangesAsync();
			return Ok(existing);
		}

		// DELETE: api/books/{id} (ta bort)
		[HttpDelete("{id:int}")]
     public async Task<IActionResult> DeleteBook(int id)
		{
          var existing = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
			if (existing is null)
			{
				return NotFound();
			}

            _db.Books.Remove(existing);
			await _db.SaveChangesAsync();
			return NoContent();
		}
	}
}

