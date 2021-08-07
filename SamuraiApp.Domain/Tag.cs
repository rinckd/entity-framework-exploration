using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SamuraiApp.Domain
{
    public class Tag
    {
        [Key]
        [Required]
        [MaxLength(40)]
        public string TagId { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}