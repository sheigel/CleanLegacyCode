using System;
using System.Collections.ObjectModel;
using System.Linq;
using LegayCode.Bll;

namespace LegayCode
{
    public class BookPresenter
    {
        private readonly IBookRepository repository;
        private readonly IBookOverview view;

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

            PublisherBookGroup bookGroup = repository.GetPublisherBookGroup(publisherQuery);
            if (!bookGroup.Books.Any())
            {
                view.ShowNoBooksPanel(String.Format("No books for the {0} publisher available.", publisherQuery));
                return;
            }
            var publisherBookGroup = FilterByClassification(bookGroup, classificationQuery);

            if (!publisherBookGroup.Books.Any())
            {
                view.ShowNoBooksPanel(String.Format("No books for the classification {0} available.", classificationQuery));
                return;
            }

            if (publisherBookGroup.Books.Count() == 1)
            {
                view.DisplayBookDetails(publisherBookGroup.Books.First());
                return;
            }

            view.DisplayGroups(new Collection<PublisherBookGroup> {publisherBookGroup});
        }

        private PublisherBookGroup FilterByClassification(PublisherBookGroup publisherBookGroup, Classification classificationQuery)
        {
            if (classificationQuery != Classification.Unknown)
            {
                publisherBookGroup.Books.Remove(b => b.Classification != classificationQuery);
            }
            return publisherBookGroup;
        }
    }
}