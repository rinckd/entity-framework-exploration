using System.Linq;
using SamuraiApp.Data;
using SamuraiApp.Services.BookServices;
using SamuraiApp.Services.Dtos;
using SamuraiApp.Services.QueryObjects;
using SamuraiApp.Tests.Builders;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace SamuraiApp.Tests
{
    public class PageFilterTests
    {
        [Theory]
        [InlineData(OrderByOptions.SimpleOrder)]
        [InlineData(OrderByOptions.ByPublicationDate)]
        public void OrderBooksBy(OrderByOptions orderByOptions)
        {
            //SETUP
            var numBooks = 5;
            var options = SqliteInMemory.CreateOptions<SamuraiContext>();
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseDummyBooks(numBooks);

            //ATTEMPT
            var service = new ListBooksService(context);
            var listOptions = new SortFilterPageOptions() {OrderByOptions = orderByOptions};
            var dtos = service.SortFilterPage(listOptions).ToList();

            //VERIFY
            dtos.Count.ShouldEqual(numBooks);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void PageBooks(int pageSize)
        {
            //SETUP
            var numBooks = 12;
            var options = SqliteInMemory.CreateOptions<SamuraiContext>();
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseDummyBooks(numBooks);

            //ATTEMPT
            var service = new ListBooksService(context);
            var listOptions = new SortFilterPageOptions() {PageSize = pageSize};
            var dtos = service.SortFilterPage(listOptions).ToList();

            //VERIFY
            dtos.Count.ShouldEqual(pageSize);
        }
    }
}