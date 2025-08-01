using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
       
        private readonly string _connectionString = "server=localhost;userid=root;password=Navyaswami@1234;database=books";

        // GET: api/books
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = new List<Book>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM librarybooks", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        Author = reader.GetString("Author"),
                        Year = reader.GetInt32("Year")
                    });
                }
            }

            return Ok(books);
        }

        // GET: api/books/1
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            Book book = null;

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM librarybooks WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    book = new Book
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        Author = reader.GetString("Author"),
                        Year = reader.GetInt32("Year")
                    };
                }
            }

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO librarybooks (Title, Author, Year) VALUES (@title, @auth, @year)", conn);
                cmd.Parameters.AddWithValue("@title", book.Title);
                cmd.Parameters.AddWithValue("@auth", book.Author);
                cmd.Parameters.AddWithValue("@year", book.Year);

                int result = cmd.ExecuteNonQuery();
                return result > 0 ? Ok("Book added successfully") : BadRequest("Insert failed");
            }
        }

        // PUT: api/books/1
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE librarybooks SET Title=@title, Author=@auth, Year=@year WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@title", updatedBook.Title);
                cmd.Parameters.AddWithValue("@auth", updatedBook.Author);
                cmd.Parameters.AddWithValue("@year", updatedBook.Year);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                return result > 0 ? Ok("Book updated") : NotFound("Book not found");
            }
        }

        // DELETE: api/books/1
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM librarybooks WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                int result = cmd.ExecuteNonQuery();
                return result > 0 ? Ok("Book deleted") : NotFound("Book not found");
            }
        }
    }
}
