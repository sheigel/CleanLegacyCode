using System;
using System.Collections.ObjectModel;
using System.Linq;
using LegayCode.Bll;

namespace LegayCode
{
    public class BookPresenter
    {
        private readonly IBookOverview view;
        private readonly IBookRepository repository;

        public BookPresenter(IBookOverview view, IBookRepository repository)
        {
            this.view = view;
            this.repository = repository;
        }

        public void DisplayFilteredBooks(Publisher publisherQuery, Classification classificationQuery)
        {
            if (publisherQuery == Publisher.Unknown)
            {
                view.DisplayGroups(repository.GetPublisherBookGroups());
                return;
            }

            PublisherBookGroup publisherBookGroup = repository.GetPublisherBookGroup(publisherQuery);
            if (publisherBookGroup == null || !publisherBookGroup.Books.Any())
            {
                view.ShowNoBooksPanel(String.Format("No books for the {0} publisher available.", publisherQuery));
                return;
            }

            if (publisherBookGroup.Books.Count() != 1)
            {
                publisherBookGroup.Books.Remove(b => b.Classification != classificationQuery);

                if (!publisherBookGroup.Books.Any())
                {
                    view.ShowNoBooksPanel("No books available for the specified filter.");
                    return;
                }
                var publisherGroups = new Collection<PublisherBookGroup> {publisherBookGroup};


                view.DisplayGroups(publisherGroups);
                return;
            }

            Book book = publisherBookGroup.Books.First();
            if (classificationQuery == Classification.Unknown || book.Classification == classificationQuery)
            {
                view.DisplayBookDetails(book);
            }
            else
            {
                //Display no books for classification
                view.ShowNoBooksPanel(String.Format("No books for the classification {0} available.", classificationQuery));
            }
        }
    }
}