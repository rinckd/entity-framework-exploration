using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Author
    {
        public long AuthorId { get; set; }
        public string Name { get; set; }
        public ICollection<BookAuthor> BooksLink { get; set; } 
    }
}