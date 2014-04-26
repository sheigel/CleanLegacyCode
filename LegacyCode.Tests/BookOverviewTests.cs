using System.Collections.ObjectModel;
using LegacyCode.Bll;
using NUnit.Framework;

namespace LegacyCode.Tests
{
    [TestFixture]
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
            }
        }
    }


    public class BookOverviewSpy : BookOverview
    {
        public BookCollection BookCollection { get; set; }

        protected override BookCollection GetBookCollection()
        {
            return BookCollection;
        }
    }
}