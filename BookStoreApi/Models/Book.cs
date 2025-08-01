using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title required")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author length can't be more than 100 characters")]
        public string Author { get; set; }

        [Range(1500, 2100, ErrorMessage = "Year must be between 1500 and 2100")]
        public int Year { get; set; }
    }
}
