using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using LegacyCode.Bll;
using NUnit.Framework;

namespace LegacyCode.Tests
{
    public class BookOverviewTests
    {
        [TestFixture]
        public class PublisherUnknown : BookOverviewTests
        {
            [Test]
            public void DisplayBooksGroupedByPublisher()
            {
                var sut = CreateSut();

                sut.DisplayFilteredBooks(Publisher.Unknown, Classification.Fiction);

                sut.DisplayedBookGroups.Should().NotBeNull();
            }
        }

        [TestFixture]
        public class PublisherGroupIsEmpty : BookOverviewTests
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
        public class PublisherGroupsHasMoreThanOneBook : BookOverviewTests
        {
            [Test]
            public void DisplayGroup()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira), A.Book.WithPublisher(Publisher.Nemira));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                sut.DisplayedBookGroups.Should().HaveCount(1);
                sut.DisplayedBookGroups.First().Books.Should().HaveCount(2);
            }
        }

        [TestFixture]
        public class PublisherGroupsHasOneBook : BookOverviewTests
        {
            [Test]
            public void DisplayErrorMessage_ClassificationNotFound()
            {
                BookOverviewSpy sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }

            [Test]
            public void DisplayBookDetails_ClassificationUnknown()
            {
                BookOverviewSpy sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Unknown);

                sut.DisplayedBook.Should().NotBeNull();
            }

            [Test]
            public void DisplayBookDetails_ClassificationFound()
            {
                BookOverviewSpy sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                sut.DisplayedBook.Should().NotBeNull();
            }
        }

        private static BookOverviewSpy CreateSut(params BookBuilder[] books)
        {
            return new BookOverviewSpy { BookCollection = A.BookCollection.WithBooks(books).Build() };
        }
    }


    public class BookOverviewSpy : BookOverview
    {
        public BookCollection BookCollection { get; set; }

        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public string ErrorMessage { get; set; }
        public Book DisplayedBook { get; set; }

        protected override BookCollection GetBookCollection()
        {
            return BookCollection;
        }

        protected override int GetPublisherId(Publisher publisherFilter)
        {
            return (int) publisherFilter;
        }

        protected override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }

        protected override void ShowNoBooksPanel(string noBooksText)
        {
            ErrorMessage = noBooksText;
        }

        protected override void DisplayBookDetails(Book book)
        {
            DisplayedBook = book;
        }
    }
}