using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using LegacyCode.Bll;
using NSubstitute;
using NUnit.Framework;

namespace LegacyCode.Tests
{
    public class BookOverviewTests
    {
        [TestFixture]
        public class PublisherUnknown : BookOverviewTests
        {
            [Test]
            public void DisplayAllGroups_UnknownClassification()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira), A.Book.WithPublisher(Publisher.Humanitas));

                sut.DisplayFilteredBooks(Publisher.Unknown, Classification.Unknown);

                sut.DisplayedBookGroups.Should().HaveCount(2);
            }

            [Test]
            public void DisplayBookDetails_OnlyOneBookMatchesBothFilters()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction),
                    A.Book.WithPublisher(Publisher.Humanitas).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Unknown, Classification.NonFiction);

                sut.DisplayedBook.Should().NotBeNull();
            }
        }

        [TestFixture]
        public class BookCollectionIsEmpty : BookOverviewTests
        {
            [Test]
            public void DisplayErrorMessage_InvalidPublisher()
            {
                var sut = CreateSut();

                var invalidPublisherId = -1;
                sut.DisplayFilteredBooks((Publisher) invalidPublisherId, Classification.Fiction);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }

            [Test]
            public void DisplayErrorMessage_PublisherNotFound()
            {
                var sut = CreateSut();

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }
        }

        [TestFixture]
        public class BookCollectionHasMoreThanOneBook : BookOverviewTests
        {
            [Test]
            public void DisplayGroupContainingBooksMatchingClassification()
            {
                const Classification matchingClassification = Classification.NonFiction;
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(matchingClassification),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(matchingClassification),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, matchingClassification);

                sut.DisplayedBookGroups.Should().HaveCount(1);
                sut.DisplayedBookGroups.First().Books.Should().HaveCount(2);
            }

            [Test]
            public void DisplayGroupContainingAllBooks_ClassificationUnknown()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Unknown);

                sut.DisplayedBookGroups.Should().HaveCount(1);
                sut.DisplayedBookGroups.First().Books.Should().HaveCount(3);
            }

            [Test]
            public void DisplayBookDetails_OnlyOneBookMatchesBothFilters()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                sut.DisplayedBook.Should().NotBeNull();
            }

            [Test]
            public void DisplayErrorMessage_ClassificationNotFound()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }
        }

        [TestFixture]
        public class BookCollectionHasOneBook : BookOverviewTests
        {
            [Test]
            public void DisplayErrorMessage_ClassificationNotFound()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }

            [Test]
            public void DisplayBookDetails_ClassificationUnknown()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Unknown);

                sut.DisplayedBook.Should().NotBeNull();
            }

            [Test]
            public void DisplayBookDetails_ClassificationFound()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                sut.DisplayedBook.Should().NotBeNull();
            }
        }

        private static BookOverviewSpy CreateSut(params BookBuilder[] books)
        {
            var bookRepository = Substitute.For<IBookRepository>();
            bookRepository.GetBookCollection().Returns(A.BookCollection.WithBooks(books).Build());
            bookRepository.GetPublisherId(Arg.Any<Publisher>()).Returns(arg => (int) arg.Arg<Publisher>());
            return new BookOverviewSpy(bookRepository);
        }
    }

    public class BookOverviewSpy : BookOverview
    {
        public BookOverviewSpy(IBookRepository bookRepository) : base(bookRepository)
        {
        }

        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }

        public string ErrorMessage { get; set; }
        public Book DisplayedBook { get; set; }

        public override void DisplayError(string noBooksText)
        {
            ErrorMessage = noBooksText;
        }

        public override void DisplayBookDetails(Book book)
        {
            DisplayedBook = book;
        }

        public override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }
    }
}