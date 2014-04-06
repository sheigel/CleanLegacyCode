using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using LegayCode.Bll;
using NUnit.Framework;

namespace LegayCode.Tests
{
    [TestFixture]
    public class PublisherUnknown
    {
        [Test]
        public void DisplaysAllGroups()
        {
            var sut = new BookOverviewSensor
            {
                PublisherGroups = new Collection<PublisherBookGroup> {A.PublisherBookGroup.Build()}
            };

            sut.FilterBooks(Publisher.Unknown, Classification.Fiction);

            sut.DisplayedBookGroups.Should().HaveCount(1);
        }
    }

    [TestFixture]
    public class PublisherGroupEmpty
    {
        [Test]
        public void DisplaysErrorMessage_NullPublisherBookGroup()
        {
            var sut = new BookOverviewSensor {PublisherBookGroup = null};

            sut.FilterBooks(Publisher.Humanitas, Classification.Fiction);

            sut.ErrorText.Should().Contain("Humanitas publisher");
        }

        [Test]
        public void DisplaysErrorMessage_EmptyPublisherBookGroup()
        {
            var sut = new BookOverviewSensor {PublisherBookGroup = A.PublisherBookGroup.Build()};

            sut.FilterBooks(Publisher.Humanitas, Classification.Fiction);

            sut.ErrorText.Should().Contain("Humanitas publisher");
        }
    }


    [TestFixture]
    public class PublisherGroupHasManyBooks
    {
        [Test]
        public void DisplaysASingleGroupForPublisher()
        {
            var sut = new BookOverviewSensor
            {
                PublisherBookGroup = A.PublisherBookGroup.WithBooks(A.Book, A.Book).Build()
            };

            sut.FilterBooks(Publisher.Humanitas, Classification.Fiction);

            sut.DisplayedBookGroups.Should().HaveCount(1);
            sut.DisplayedBookGroups.First().Books.Should().HaveCount(2);
        }
    }

    [TestFixture]
    public class PublisherGroupHasOneBook
    {
        [SetUp]
        public void SetUp()
        {
            bookClassification = Classification.Fiction;
            sut = new BookOverviewSensor
            {
                PublisherBookGroup =
                    A.PublisherBookGroup.WithBooks(A.Book.WithClassification(bookClassification)).Build()
            };
        }

        private BookOverviewSensor sut;
        private Classification bookClassification;

        [Test]
        public void DisplaysErrorMessage_BookDoesNotMatchClassification()
        {
            sut.FilterBooks(Publisher.Humanitas, Classification.NonFiction);

            sut.ErrorText.Should().Contain("classification NonFiction available");
        }

        [Test]
        public void DisplaysBookDetails_NoSpecifiedClassification()
        {
            sut.FilterBooks(Publisher.Humanitas, Classification.Unknown);

            sut.DisplayedBook.Should().NotBeNull();
        }

        [Test]
        public void DisplaysBookDetails_BookMatchesClassification()
        {
            sut.FilterBooks(Publisher.Humanitas, bookClassification);

            sut.DisplayedBook.Should().NotBeNull();
        }
    }


    public class BookOverviewSensor : BookOverview
    {
        public Collection<PublisherBookGroup> PublisherGroups { get; set; }
        public PublisherBookGroup PublisherBookGroup { get; set; }
        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public string ErrorText { get; set; }
        public Book DisplayedBook { get; set; }


        protected override Collection<PublisherBookGroup> GetPublisherBookGroups()
        {
            return PublisherGroups;
        }

        protected override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }

        protected override void DisplayBookDetails(Book book)
        {
            DisplayedBook = book;
        }

        protected override PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery)
        {
            return PublisherBookGroup;
        }

        protected override void ShowNoBooksPanel(string noBooksText)
        {
            ErrorText = noBooksText;
        }
    }
}