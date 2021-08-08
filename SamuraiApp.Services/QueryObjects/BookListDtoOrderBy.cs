using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SamuraiApp.Services.Dtos;

namespace SamuraiApp.Services.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")] SimpleOrder = 0,
        [Display(Name = "Votes ↑")] ByVotes,
        [Display(Name = "Publication Date ↑")] ByPublicationDate,
        [Display(Name = "Price ↓")] ByPriceLowestFirst,
        [Display(Name = "Price ↑")] ByPriceHighestFirst
    }
    
    public static class BookListDtoOrderBy
    {
        public static IQueryable<BookListDto> OrderBooksBy
        (this IQueryable<BookListDto> books,
            OrderByOptions orderByOptions)
        {
            return orderByOptions switch
            {
                OrderByOptions.SimpleOrder => //#A
                    books.OrderByDescending( //#A
                        x => x.BookId) //#A
                ,
                OrderByOptions.ByVotes => //#B
                    books.OrderByDescending(x => //#B
                        x.ReviewsAverageVotes) //#B
                ,
                OrderByOptions.ByPublicationDate => //#C
                    books.OrderByDescending( //#C
                        x => x.PublishedOn) //#C
                ,
                OrderByOptions.ByPriceLowestFirst => //#D
                    books.OrderBy(x => x.ActualPrice) //#D
                ,
                OrderByOptions.ByPriceHighestFirst => //#D
                    books.OrderByDescending( //#D
                        x => x.ActualPrice) //#D
                ,
                _ => throw new ArgumentOutOfRangeException(nameof(orderByOptions), orderByOptions, null)
            };
        }

        /************************************************************
        #A Because of paging we always need to sort. I default to showing latest entries first
        #B This orders the book by votes. Books without any votes (null return) go at the bottom
        #C Order by publication date - latest books at the top
        #D Order by actual price, which takes into account any promotional price - both lowest first and highest first
         * ********************************************************/
    }
}