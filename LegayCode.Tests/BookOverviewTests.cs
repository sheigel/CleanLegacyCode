using System.Collections.ObjectModel;
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
                PublisherGroups =
                    new Collection<PublisherBookGroup> {new PublisherBookGroup(new BookCollection(), 2, "Humanitas")}
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
            var sut = new BookOverviewSensor
            {
                PublisherBookGroup = new PublisherBookGroup(new BookCollection(), 2, "unimportant")
            };

            sut.FilterBooks(Publisher.Humanitas);

            sut.ErrorText.Should().Contain("Humanitas publisher");
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