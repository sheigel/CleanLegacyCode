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
        private BookPresenter presenter;

        [Test]
        public void DisplaysAllGroups()
        {
            var sensor = new BookOverviewSensor {PublisherGroups = new Collection<PublisherBookGroup> {A.PublisherBookGroup.Build()}};
            presenter = new BookPresenter(sensor, sensor);

            presenter.DisplayFilteredBooks(Publisher.Unknown, Classification.Fiction);

            sensor.DisplayedBookGroups.Should().HaveCount(1);
        }
    }

    [TestFixture]
    public class PublisherGroupEmpty
    {
        private BookPresenter sut;

        [Test]
        public void DisplaysErrorMessage_NullPublisherBookGroup()
        {
            var sensor = new BookOverviewSensor();
            sut = new BookPresenter(sensor, sensor);
            sensor.PublisherBookGroup = null;

            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.Fiction);

            sensor.ErrorText.Should().Contain("Humanitas publisher");
        }

        [Test]
        public void DisplaysErrorMessage_EmptyPublisherBookGroup()
        {
            var sensor = new BookOverviewSensor();
            sut = new BookPresenter(sensor, sensor);
            sensor.PublisherBookGroup = A.PublisherBookGroup.Build();

            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.Fiction);

            sensor.ErrorText.Should().Contain("Humanitas publisher");
        }
    }


    [TestFixture]
    public class PublisherGroupHasManyBooks
    {
        private BookPresenter sut;

        [Test]
        public void DisplaysASingleGroupForPublisher_MatchingClassification()
        {
            var sensor = new BookOverviewSensor();
            sut = new BookPresenter(sensor, sensor);
            sensor.PublisherBookGroup =
                A.PublisherBookGroup.WithBooks(A.Book.WithClassification(Classification.Fiction),
                    A.Book.WithClassification(Classification.Fiction)).Build();

            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.Fiction);

            sensor.DisplayedBookGroups.Should().HaveCount(1);
            sensor.DisplayedBookGroups.First().Books.Should().HaveCount(2);
        }

        [Test]
        public void DisplaysOnlyBooksMatchingClassification()
        {
            var sensor = new BookOverviewSensor();
            sut = new BookPresenter(sensor, sensor);
            sensor.PublisherBookGroup =
                A.PublisherBookGroup.WithBooks(A.Book.WithClassification(Classification.NonFiction),
                    A.Book.WithClassification(Classification.Fiction), A.Book.WithClassification(Classification.Fiction)).Build();

            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.Fiction);

            sensor.DisplayedBookGroups.Should().HaveCount(1);
            sensor.DisplayedBookGroups.First().Books.Should().HaveCount(2);
        }
    }

    [TestFixture]
    public class PublisherGroupHasOneBook
    {
        [SetUp]
        public void SetUp()
        {
            bookClassification = Classification.Fiction;
            sensor = new BookOverviewSensor();
            sensor.PublisherBookGroup = A.PublisherBookGroup.WithBooks(A.Book.WithClassification(bookClassification)).Build();
            sut = new BookPresenter(sensor, sensor);
        }

        private BookPresenter sut;
        private Classification bookClassification;
        private BookOverviewSensor sensor;

        [Test]
        public void DisplaysErrorMessage_BookDoesNotMatchClassification()
        {
            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.NonFiction);

            sensor.ErrorText.Should().Contain("classification NonFiction available");
        }

        [Test]
        public void DisplaysBookDetails_NoSpecifiedClassification()
        {
            sut.DisplayFilteredBooks(Publisher.Humanitas, Classification.Unknown);

            sensor.DisplayedBook.Should().NotBeNull();
        }

        [Test]
        public void DisplaysBookDetails_BookMatchesClassification()
        {
            sut.DisplayFilteredBooks(Publisher.Humanitas, bookClassification);

            sensor.DisplayedBook.Should().NotBeNull();
        }
    }


    public class BookOverviewSensor : IBookOverview, IBookRepository

    {
        public Collection<PublisherBookGroup> PublisherGroups { get; set; }
        public PublisherBookGroup PublisherBookGroup { get; set; }
        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public string ErrorText { get; set; }
        public Book DisplayedBook { get; set; }


        public void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }

        public void DisplayBookDetails(Book book)
        {
            DisplayedBook = book;
        }

        public void ShowNoBooksPanel(string noBooksText)
        {
            ErrorText = noBooksText;
        }

        public Collection<PublisherBookGroup> GetPublisherBookGroups()
        {
            return PublisherGroups;
        }

        public PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery)
        {
            return PublisherBookGroup;
        }
    }
}