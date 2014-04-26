using System.Linq;
using LegacyCode.Bll;

namespace LegacyCode
{
    public class BookOverviewPresenter
    {
        private BookOverview view;
        private IBookRepository bookRepository;

        public BookOverviewPresenter(IBookRepository bookRepository, BookOverview bookOverview)
        {
            this.bookRepository = bookRepository;
            view = bookOverview;
        }

        public void DisplayFilteredBooks(Publisher publisherFilter, Classification classificationFilter)
        {
            var bookCollection = FilterBooks(publisherFilter, classificationFilter);

            DisplayBooks(bookCollection);
        }

        private BookCollection FilterBooks(Publisher publisherFilter, Classification classificationFilter)
        {
            var bookCollection = bookRepository.GetBookCollection();

            if (publisherFilter != Publisher.Unknown)
            {
                var publisherId = bookRepository.GetPublisherId(publisherFilter);
                bookCollection = bookCollection.WherePublisher(publisherId);
            }

            return bookCollection.WhereClassification(classificationFilter);
        }

        private void DisplayBooks(BookCollection bookCollection)
        {
            if (bookCollection.Count == 0)
            {
                view.DisplayError("We couldn't find any books matching your filter.");
                return;
            }
            if (bookCollection.Count == 1)
            {
                view.DisplayBookDetails(bookCollection.First());
            }
            else
            {
                view.DisplayGroups(bookCollection.GetGroups);
            }
        }
    }
}