using System.Collections.ObjectModel;
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
                var sut = new BookOverviewSpy();
                sut.BookCollection = new BookCollection();
                
                sut.DisplayFilteredBooks(Publisher.Unknown);

                sut.DisplayedBookGroups.Should().NotBeNull();
            }
        }
    }


    public class BookOverviewSpy : BookOverview
    {
        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public BookCollection BookCollection { get; set; }

        protected override BookCollection GetBookCollection()
        {
            return BookCollection;
        }
        protected override void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            DisplayedBookGroups = publisherBookGroups;
        }
    }
}