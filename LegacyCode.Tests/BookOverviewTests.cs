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
                var sut = CreateSut();

                sut.DisplayFilteredBooks(Publisher.Unknown);

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
                sut.DisplayFilteredBooks((Publisher) invalidPublisherId);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }

            [Test]
            public void DisplayErrorMessage_PublisherNotFound()
            {
                var sut = CreateSut();

                sut.DisplayFilteredBooks(Publisher.Nemira);

                sut.ErrorMessage.Should().Contain("couldn't find");
            }
        }

        private static BookOverviewSpy CreateSut()
        {
            var sut = new BookOverviewSpy();
            sut.BookCollection = new BookCollection();
            return sut;
        }
    }


    public class BookOverviewSpy : BookOverview
    {
        public BookCollection BookCollection { get; set; }

        public Collection<PublisherBookGroup> DisplayedBookGroups { get; set; }
        public string ErrorMessage { get; set; }

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
    }
}