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
        public void DisplayAllGroups()
        {
            var sut = new BookOverviewSensor
            {
                PublisherGroups = new Collection<PublisherBookGroup> {A.PublisherBookGroup.Build()}
            };

            sut.FilterBooks(Publisher.Unknown);

            sut.DisplayedBookGroups.Should().HaveCount(1);
        }
    }

    [TestFixture]
    public class PublisherGroupEmpty
    {
        [Test]
        public void DisplayErrorMessage_NullPublisherBookGroup()
        {
            var sut = new BookOverviewSensor {PublisherBookGroup = null};

            sut.FilterBooks(Publisher.Humanitas);

            sut.ErrorText.Should().Contain("Humanitas publisher");
        }

        [Test]
        public void DisplayErrorMessage_EmptyPublisherBookGroup()
        {
            var sut = new BookOverviewSensor {PublisherBookGroup = A.PublisherBookGroup.Build()};

            sut.FilterBooks(Publisher.Humanitas);

            sut.ErrorText.Should().Contain("Humanitas publisher");
        }
    }


    [TestFixture]
    public class PublisherGroupHasManyBooks
    {
        [Test]
        public void DisplayErrorMessage_NullPublisherBookGroup()
        {
            var sut = new BookOverviewSensor
            {
                PublisherBookGroup = A.PublisherBookGroup.WithBooks(A.Book, A.Book).Build()
            };

            sut.FilterBooks(Publisher.Humanitas);

            sut.DisplayedBookGroups.Should().HaveCount(1);
            sut.DisplayedBookGroups.First().Books.Should().HaveCount(2);
        }
    }


    public class BookOverviewSensor : BookOverview
    {
        public Collection<PublisherBookGroup> PublisherGroups { get; set; }
        public PublisherBookGroup PublisherBookGroup { get; set; }
        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public string ErrorText { get; set; }


        protected override Collection<PublisherBookGroup> GetPublisherBookGroups()
        {
            return PublisherGroups;
        }

        protected override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
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