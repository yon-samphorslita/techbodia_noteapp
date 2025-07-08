using Microsoft.AspNetCore.Mvc;
using NoteAppApi.Models;
using NoteAppApi.Repositories;

namespace NoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;

        public NoteController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _noteRepository.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title))
            {
                return BadRequest("Invalid note data.");
            }
                note.CreatedAt = DateTime.UtcNow;
    note.UpdatedAt = DateTime.UtcNow;

            var createdNoteId = await _noteRepository.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new { id = createdNoteId }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note note)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title) || note.Id != id)
            {
                return BadRequest("Invalid note data.");
            }
            var updated = await _noteRepository.UpdateNoteAsync(note);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var deleted = await _noteRepository.DeleteNoteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}