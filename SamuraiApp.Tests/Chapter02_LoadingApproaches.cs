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
        }
        
        [Fact]
        public void TestEagerLoadBookAllOk()
        {
            // 2.4.1 Eager Loading
            
            //SETUP
            var options = SqliteInMemory.CreateOptions<SamuraiContext>();
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();

            context.ChangeTracker.Clear();

            //ATTEMPT
            var firstBook = context.Books
                .Include(book => book.AuthorsLink) //#A
                .ThenInclude(bookAuthor => bookAuthor.Author) //#B                    
                .Include(book => book.Reviews) //#C
                .Include(book => book.Tags) //#D
                .Include(book => book.Promotion) //#E
                .First(); //#F
            /*********************************************************
                #A The first Include() gets a collection of BookAuthor
                #B The ThenInclude() gets the next link, in this case the link to the Author
                #C The Include() gets a collection of Reviews, which may be an empty collection
                #D This loads the Tags. Note that this directly accesses the Tags
                #E This loads any optional PriceOffer class, if one is assigned
                #F This takes the first book
                * *******************************************************/

            //VERIFY
            firstBook.AuthorsLink.ShouldNotBeNull();
            firstBook.AuthorsLink.First()
                .Author.ShouldNotBeNull();

            firstBook.Reviews.ShouldNotBeNull();
        }
        
                [Fact]
        public void TestExplicitLoadBookOk()
        {
            // Explicit Loading 2.4.2
            
            // Explicit loading can also be useful when you need that
            // related data in only some circumstances. You might also find explicit loading to be useful
            // in complex business logic because you can leave the job of loading the specific relationships to the
            // parts of the business logic that need it.
            // The downside of explicit loading is more database round trips, which can be inefficient. 
            //SETUP
            var options = SqliteInMemory.CreateOptions<SamuraiContext>();
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();

            context.ChangeTracker.Clear();

            //ATTEMPT
            var firstBook = context.Books.First();            //#A
            context.Entry(firstBook)
                .Collection(book => book.AuthorsLink).Load(); //#B
            foreach (var authorLink in firstBook.AuthorsLink) //#C
            {
                //#C
                context.Entry(authorLink)                     //#C
                    .Reference(bookAuthor =>                  //#C
                        bookAuthor.Author).Load();            //#C
            }                                                 //#C

            context.Entry(firstBook)                          //#D
                .Collection(book => book.Reviews).Load();     //#D
            context.Entry(firstBook)  //#E
                .Collection(book => book.Tags).Load(); //#E
            context.Entry(firstBook)                          //#F
                .Reference(book => book.Promotion).Load();    //#F
            /*********************************************************
                #A This reads in the first book on its own
                #B This explicitly loads the linking table, BookAuthor
                #C To load all the possible Authors it has to loop through all the BookAuthor entries and load each linked Author class
                #D This loads all the Reviews
                #E This loads the Tags
                #F This loads the optional PriceOffer class
                * *******************************************************/

            //VERIFY
            firstBook.AuthorsLink.ShouldNotBeNull();
            firstBook.AuthorsLink.First()
                .Author.ShouldNotBeNull();

            firstBook.Reviews.ShouldNotBeNull();
        }
        
        [Fact]
        public void TestSelectLoadBookOk()
        {
            // Select Loading example.
            // See 2.4.3
            // The advantage of this approach is that only the data you need is loaded,
            // which can be more efficient if you don’t need all the data. 
            
            
            //SETUP
            var showLog = false;
            var options = SqliteInMemory.CreateOptionsWithLogTo<SamuraiContext>(log =>
            {
                if (showLog)
                    _output.WriteLine(log);
            });
            using var context = new SamuraiContext(options);
            context.Database.EnsureCreated();
            context.SeedDatabaseFourBooks();

            context.ChangeTracker.Clear();

            //ATTEMPT
            showLog = true;
            var books = context.Books
                .Select(book => new //#A
                    {
                        //#A
                        book.Title, //#B
                        book.Price, //#B
                        NumReviews //#C
                            = book.Reviews.Count, //#C
                    }
                ).ToList();
            /*********************************************************
                #A This uses the LINQ select keyword and creates an anonymous type to hold the results
                #B These are simple copies of a couple of properties
                #C This runs a query that counts the number of reviews
                * *******************************************************/

            //VERIFY
            showLog = false;
            books.First().NumReviews.ShouldEqual(0);
        }
    }
}