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
                var sut = new BookOverview();

                sut.DisplayFilteredBooks();

            }
        }
    }
}