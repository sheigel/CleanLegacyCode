using System.Collections.ObjectModel;
using System.Linq;
using LegacyCode.Bll;
using NSubstitute;
using NUnit.Framework;

namespace LegacyCode.Tests
{
    public class BookOverviewPresenterTests
    {
        [TestFixture]
        public class PublisherUnknown : BookOverviewPresenterTests
        {
            [Test]
            public void DisplayAllGroups_UnknownClassification()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira), A.Book.WithPublisher(Publisher.Humanitas));

                sut.DisplayFilteredBooks(Publisher.Unknown, Classification.Unknown);

                view.Received().DisplayGroups(Arg.Is<Collection<PublisherBookGroup>>(g => g.Count == 2));
            }

            [Test]
            public void DisplayBookDetails_OnlyOneBookMatchesBothFilters()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction),
                    A.Book.WithPublisher(Publisher.Humanitas).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Unknown, Classification.NonFiction);

                view.Received().DisplayBookDetails(Arg.Is<Book>(b => b != null));
            }
        }

        [TestFixture]
        public class BookCollectionIsEmpty : BookOverviewPresenterTests
        {
            [Test]
            public void DisplayErrorMessage_InvalidPublisher()
            {
                var sut = CreateSut();

                var invalidPublisherId = -1;
                sut.DisplayFilteredBooks((Publisher) invalidPublisherId, Classification.NonFiction);

                view.Received().DisplayError(Arg.Is<string>(m => m.Contains("couldn't find")));
            }

            [Test]
            public void DisplayErrorMessage_PublisherNotFound()
            {
                var sut = CreateSut();

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                view.Received().DisplayError(Arg.Is<string>(m => m.Contains("couldn't find")));
            }
        }

        [TestFixture]
        public class BookCollectionHasOneBook : BookOverviewPresenterTests
        {
            [Test]
            public void DisplayErrorMessage_ClassificationNotFound()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                view.Received().DisplayError(Arg.Is<string>(m => m.Contains("couldn't find")));
            }

            [Test]
            public void DisplayBookDetails_ClassificationUnknown()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Unknown);

                view.Received().DisplayBookDetails(Arg.Is<Book>(b => b != null));
            }

            [Test]
            public void DisplayBookDetails_ClassificationFound()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                view.Received().DisplayBookDetails(Arg.Is<Book>(b => b != null));
            }
        }

        [TestFixture]
        public class BookCollectionHasMoreThanOneBook : BookOverviewPresenterTests
        {
            [Test]
            public void DisplayGroupContainingBooksMatchingClassification()
            {
                var matchingClassification = Classification.NonFiction;
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(matchingClassification),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(matchingClassification),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, matchingClassification);

                view.Received().DisplayGroups(Arg.Is<Collection<PublisherBookGroup>>(c => c.Count == 1 && c.First().Books.Count == 2));
            }

            [Test]
            public void DisplayGroupContainingAllBooks_ClassificationUnknown()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.Unknown);

                view.Received().DisplayGroups(Arg.Is<Collection<PublisherBookGroup>>(c => c.Count == 1 && c.First().Books.Count == 3));
            }

            [Test]
            public void DisplayBookDetails_OnlyOneBookMatchesBothFilters()
            {
                var sut = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.Fiction));

                sut.DisplayFilteredBooks(Publisher.Nemira, Classification.NonFiction);

                view.Received().DisplayBookDetails(Arg.Is<Book>(b => b != null));
            }

            [Test]
            public void DisplayErrorMessage_ClassificationNotFound()
            {
                var presenter = CreateSut(A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction),
                    A.Book.WithPublisher(Publisher.Nemira).WithClassification(Classification.NonFiction));

                presenter.DisplayFilteredBooks(Publisher.Nemira, Classification.Fiction);

                view.Received().DisplayError(Arg.Is<string>(m => m.Contains("couldn't find")));
            }
        }

        private BookOverviewPresenter CreateSut(params BookBuilder[] books)
        {
            var bookRepository = Substitute.For<IBookRepository>();
            bookRepository.GetBookCollection().Returns(A.BookCollection.WithBooks(books).Build());
            bookRepository.GetPublisherId(Arg.Any<Publisher>()).Returns(arg => (int) arg.Arg<Publisher>());
            return new BookOverviewPresenter(bookRepository, view);
        }

        private IBookOverviewView view;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IBookOverviewView>();
        }
    }
}