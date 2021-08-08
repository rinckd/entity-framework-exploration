using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Services.Dtos;
using SamuraiApp.Services.QueryObjects;

namespace SamuraiApp.Services.BookServices
{
    public class ListBooksService
    {
        private readonly SamuraiContext _context;

        public ListBooksService(SamuraiContext context)
        {
            _context = context;
        }

        public IQueryable<BookListDto> SortFilterPage
            (SortFilterPageOptions options)
        {
            var booksQuery = _context.Books //#A
                .AsNoTracking() //#B
                .MapBookToDto() //#C
                .OrderBooksBy(options.OrderByOptions) //#D
                .FilterBooksBy(options.FilterBy, //#E
                    options.FilterValue); //#E

            options.SetupRestOfDto(booksQuery); //#F

            return booksQuery.Page(options.PageNum - 1, //#G
                options.PageSize); //#G
        }
        /*********************************************************
        #A This starts by selecting the Books property in the Application's DbContext 
        #B Because this is a read-only query I add .AsNoTracking(). It makes the query faster
        #C It then uses the Select query object which will pick out/calculate the data it needs
        #D It then adds the commands to order the data using the given options
        #E Then it adds the commands to filter the data
        #F This stage sets up the number of pages and also makes sure PageNum is in the right range
        #G Finally it applies the paging commands
        ******************************************************/
    }
}