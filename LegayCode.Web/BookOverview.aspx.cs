﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI;
using LegayCode.Bll;

namespace LegayCode
{
    public partial class BookOverview : Page
    {
        private static Publisher QueryStringPublisher
        {
            get { throw new DependencyException(); }
        }

        private static Classification QueryStringBookClassification
        {
            get { throw new DependencyException(); }
        }

        private static Uri GetTargetPage(long bookId, Publisher publisher)
        {
            throw new DependencyException();
        }

        protected virtual void ShowNoBooksPanel(string noBooksText)
        {
            errorLabel.Text = Resources.GetString(noBooksText);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FilterBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public void FilterBooks(Publisher publisherQuery, Classification classificationQuery)
        {
            if (publisherQuery == Publisher.Unknown)
            {
                DisplayGroups(GetPublisherBookGroups());
                return;
            }

            PublisherBookGroup publisherBookGroup = GetPublisherBookGroup(publisherQuery);
            if (publisherBookGroup == null || !publisherBookGroup.Books.Any())
            {
                ShowNoBooksPanel(string.Format("No books for the {0} publisher available.", publisherQuery));
                return;
            }

            if (publisherBookGroup.Books.Count() != 1)
            {
                var publisherGroups = new Collection<PublisherBookGroup> {publisherBookGroup};
                DisplayGroups(publisherGroups);
                return;
            }

            Book book = publisherBookGroup.Books.First();
            if (classificationQuery == Classification.Unknown || book.Classification == classificationQuery)
            {
                DisplayBookDetails(book);
            }
            else
            {
                //Display no books for classification
                ShowNoBooksPanel(string.Format("No books for the classification {0} available.", classificationQuery));
            }
        }

        protected virtual void DisplayBookDetails(Book book)
        {
//Display book details
            Uri targetPage = GetTargetPage(book.ISBN, book.Publisher);

            Response.Redirect(targetPage.ToString());
        }

        protected virtual PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery)
        {
            return BookManager.GetBookCollection().GetPublisherGroup(publisherQuery);
        }

        protected virtual Collection<PublisherBookGroup> GetPublisherBookGroups()
        {
            return BookManager.GetBookCollection().GetGroups;
        }

        protected virtual void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
//Display groups
            gridView.DataSource = publisherBookGroups;
            gridView.DataBind();
        }
    }
}