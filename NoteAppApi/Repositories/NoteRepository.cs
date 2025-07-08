using Dapper;
using NoteAppApi.Data;
using NoteAppApi.Models;

namespace NoteAppApi.Repositories
{
    public class NoteRepository 
    {
        private readonly DapperContext _context;

        public NoteRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            var query = "SELECT * FROM Notes ORDER BY CreatedAt DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Note>(query);
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            var query = "SELECT * FROM Notes WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Note>(query, new { Id = id });
        }

        public async Task<int> CreateNoteAsync(Note note)
        {
            var query = "INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt, UserId) " +
                        "VALUES (@Title, @Content, @CreatedAt, @UpdatedAt, @UserId); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, note);
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            var query = "UPDATE Notes SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, note);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            var query = "DELETE FROM Notes WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}