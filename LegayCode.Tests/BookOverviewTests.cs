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
            var sut = new BookOverviewSensor()
            {
                PublisherGroups =
                    new Collection<PublisherBookGroup> {new PublisherBookGroup(new BookCollection(), 2, "Humanitas")}
            };

            sut.FilterBooks(Publisher.Unknown);

            sut.DisplayedBookGroups.Should().HaveCount(1);
        }
    }

    public class BookOverviewSensor : BookOverview
    {
        public Collection<PublisherBookGroup> PublisherGroups { get; set; }
        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }


        protected override Collection<PublisherBookGroup> PublisherBookGroups()
        {
            return PublisherGroups;
        }

        protected override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }
    }


}