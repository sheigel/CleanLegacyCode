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
            BookCollection bookCollection;
            if (publisherQuery == Publisher.Unknown)
            {
                bookCollection = repository.GetBookCollection();
                view.DisplayGroups(bookCollection.GetGroups);
            }
           else {
               bookCollection = repository.GetPublisherBookGroup(publisherQuery).Books;
               if (!bookCollection.Any())
                {
                    view.ShowNoBooksPanel(String.Format("No books for the {0} publisher available.", publisherQuery));
                    return;
                }
                FilterByClassification(classificationQuery, bookCollection);

                if (!bookCollection.Any())
                {
                    view.ShowNoBooksPanel(String.Format("No books for the classification {0} available.", classificationQuery));
                    return;
                }

                if (bookCollection.Count() == 1)
                {
                    view.DisplayBookDetails(bookCollection.First());
                    return;
                }

            
                view.DisplayGroups( bookCollection.GetGroups );
            }
        }

        private static void FilterByClassification(Classification classificationQuery, BookCollection bookCollection)
        {
            if (classificationQuery != Classification.Unknown)
            {
                bookCollection.Remove(b => b.Classification != classificationQuery);
            }
        }
    }
}