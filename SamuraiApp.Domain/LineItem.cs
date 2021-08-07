using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.Domain
{
    public class LineItem : IValidatableObject
    {
        public int LineItemId { get; set; }

        [Range(1,5, ErrorMessage =                      //#B
            "This order is over the limit of 5 books.")] //#B
        public byte LineNum { get; set; }

        public short NumBooks { get; set; }

        /// <summary>
        /// This holds a copy of the book price. We do this in case the price of the book changes,
        /// e.g. if the price was discounted in the future the order is still correct.
        /// </summary>
        public decimal BookPrice { get; set; }

        // relationships

        public int OrderId { get; set; }
        public int BookId { get; set; }

        public Book ChosenBook { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate //#C
            (ValidationContext validationContext)                 //#C
        {
            var currContext = 
                validationContext.GetService(typeof(DbContext));//#D

            if (ChosenBook.Price < 0)                      //#E
                yield return new ValidationResult(         //#E
                    $"Sorry, the book '{ChosenBook.Title}' is not for sale."); //#E

            if (NumBooks > 100) //#F
                yield return new ValidationResult(//#F
                    "If you want to order a 100 or more books"+       //#F
                    " please phone us on 01234-5678-90",              //#F
                    new[] { nameof(NumBooks) });  //#G
        }
    }
}