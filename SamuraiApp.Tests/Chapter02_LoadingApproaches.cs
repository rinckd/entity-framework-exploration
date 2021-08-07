using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Tests.Builders;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace SamuraiApp.Tests
{
    public class Chapter02_LoadingApproaches
    {
        private readonly ITestOutputHelper _output;
        public Chapter02_LoadingApproaches(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void TestAsNoTrackingBookCountAuthorsOk()
        {
            // Arrange
            var options = SqliteInMemory.CreateOptions<SamuraiContext>();
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();

            context.ChangeTracker.Clear();

            // Act
            var books = context.Books
                .AsNoTracking()
                .Include(book => book.AuthorsLink) 
                .ThenInclude(book => book.Author)
                .ToList();

            // Assert
            books.SelectMany(x => x.AuthorsLink.Select(y => y.Author)).Distinct().Count().ShouldEqual(4);
            var test = 1;
        }
    }
}