using RedRiverTest.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedRiverTest.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace RedRiverTest.Api.Controllers
{

 [Route("api/my-citations")]
	[ApiController]
	[Authorize]
	public class CitationsController : ControllerBase
	{

		private readonly AppDbContext _db;

		public CitationsController(AppDbContext db)

		{
			_db = db;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Citation>>> GetCitations()
		{
          var userId = User.Identity?.Name;
			if (!string.IsNullOrWhiteSpace(userId))
			{
				var hasAny = await _db.Citations.AsNoTracking().AnyAsync(c => c.UserId == userId);
				if (!hasAny)
				{
					_db.Citations.AddRange(
						new Citation { Text = "Stay hungry, stay foolish.", Author = "Steve Jobs", UserId = userId },
						new Citation { Text = "Simplicity is the soul of efficiency.", Author = "Austin Freeman", UserId = userId },
						new Citation { Text = "First, solve the problem. Then, write the code.", Author = "John Johnson", UserId = userId },
						new Citation { Text = "Talk is cheap. Show me the code.", Author = "Linus Torvalds", UserId = userId },
						new Citation { Text = "Make it work, make it right, make it fast.", Author = "Kent Beck", UserId = userId }
					);
					await _db.SaveChangesAsync();
				}
			}
			var query = _db.Citations.AsNoTracking().OrderBy(c => c.Id);
			if (!string.IsNullOrWhiteSpace(userId))
			{
				query = query.Where(c => c.UserId == userId).OrderBy(c => c.Id);
			}

			var citations = await query.ToListAsync();
			return Ok(citations);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Citation>> GetCitation(int id)
		{
           var userId = User.Identity?.Name;
			var citation = await _db.Citations
				.AsNoTracking()
               .FirstOrDefaultAsync(c => c.Id == id);
			if (citation is null)
			{
				return NotFound();
			}
			if (!string.IsNullOrWhiteSpace(userId) && citation.UserId != userId)
			{
				return NotFound();
			}

         return Ok(citation);
		}

		[HttpPost]
		public async Task<ActionResult<Citation>> CreateCitation(Citation citation)
		{
          var userId = User.Identity?.Name;
			citation.Id = 0;
            citation.UserId = userId ?? string.Empty;
			_db.Citations.Add(citation);
			await _db.SaveChangesAsync();
			return CreatedAtAction(nameof(GetCitation), new { id = citation.Id }, citation);
			
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<Citation>> UpdateCitation(int id, Citation citation)
		{
          var userId = User.Identity?.Name;
			var existing = await _db.Citations.FirstOrDefaultAsync(c => c.Id == id);
			if (existing is null)
			{
				return NotFound();
			}
			if (!string.IsNullOrWhiteSpace(userId) && existing.UserId != userId)
			{
				return NotFound();
			}

          existing.Author = citation.Author;
			existing.Text = citation.Text;
			await _db.SaveChangesAsync();
			return Ok(existing);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteCitation(int id)
		{
            var userId = User.Identity?.Name;
			var existing = await _db.Citations.FirstOrDefaultAsync(c => c.Id == id);
				if (existing is null)
				{
					return NotFound();
				}
				if (!string.IsNullOrWhiteSpace(userId) && existing.UserId != userId)
				{
					return NotFound();
				}

				_db.Citations.Remove(existing);
				await _db.SaveChangesAsync();
				return NoContent();
		}

	}
}
