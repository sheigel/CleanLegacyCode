using System;
using System.Collections.ObjectModel;
using System.Linq;
using LegayCode.Bll;

namespace LegayCode
{
    public class BookPresenter
    {
        private readonly IBookOverview bookOverview;
        private readonly IBookRepository bookRepository;

        public BookPresenter(IBookOverview bookOverview, IBookRepository bookRepository)
        {
            this.bookOverview = bookOverview;
            this.bookRepository = bookRepository;
        }

        public void DisplayFilteredBooks(Publisher publisherQuery, Classification classificationQuery)
        {
            if (publisherQuery == Publisher.Unknown)
            {
                bookOverview.DisplayGroups(bookRepository.GetPublisherBookGroups());
                return;
            }

            PublisherBookGroup publisherBookGroup = bookRepository.GetPublisherBookGroup(publisherQuery);
            if (publisherBookGroup == null || !publisherBookGroup.Books.Any())
            {
                bookOverview.ShowNoBooksPanel(String.Format("No books for the {0} publisher available.", publisherQuery));
                return;
            }

            if (publisherBookGroup.Books.Count() != 1)
            {
                publisherBookGroup.Books.Remove(b => b.Classification != classificationQuery);
                var publisherGroups = new Collection<PublisherBookGroup> {publisherBookGroup};

                bookOverview.DisplayGroups(publisherGroups);
                return;
            }

            Book book = publisherBookGroup.Books.First();
            if (classificationQuery == Classification.Unknown || book.Classification == classificationQuery)
            {
                bookOverview.DisplayBookDetails(book);
            }
            else
            {
                //Display no books for classification
                bookOverview.ShowNoBooksPanel(String.Format("No books for the classification {0} available.", classificationQuery));
            }
        }
    }
}