using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI;
using LegacyCode.Bll;

namespace LegacyCode
{
    public partial class BookOverview : Page
    {
        private static Publisher QueryStringPublisher { get { throw new DependencyException(); } }

        private static Classification QueryStringBookClassification { get { throw new DependencyException(); } }

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
            DisplayFilteredBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public void DisplayFilteredBooks(Publisher publisherFilter, Classification classificationFilter)
        {
            if (publisherFilter == Publisher.Unknown)
            {
                DisplayGroups(GetBookCollection().GetGroups);
                return;
            }

            var publisherId = GetPublisherId(publisherFilter);
            var publisherBookGroup = GetBookCollection().GetPublisherGroupById(publisherId);

            if (publisherBookGroup == null || publisherBookGroup.Books.Count() == 0)
            {
                ShowNoBooksPanel("We couldn't find any books matching your filter.");
                return;
            }

            if (publisherBookGroup.Books.Count() == 1)
            {
                var book = publisherBookGroup.Books.First();

                if (classificationFilter == Classification.Unknown || book.Classification == classificationFilter)
                {
                    DisplayBookDetails(book);
                }
                else
                {
                    ShowNoBooksPanel("We couldn't find any books matching your filter.");
                }
            }
            else
            {
                if (classificationFilter != Classification.Unknown)
                {
                    publisherBookGroup.Books.Remove(b => b.Classification != classificationFilter);
                }

                if (publisherBookGroup.Books.Count() == 1)
                {
                    var book = publisherBookGroup.Books.First();
                    DisplayBookDetails(book);
                }

                var publisherGroups = new Collection<PublisherBookGroup> {publisherBookGroup};
                
                DisplayGroups(publisherGroups);
            }
        }

        protected virtual void DisplayBookDetails(Book book)
        {
            var targetPage = GetTargetPage(book.ISBN, book.Publisher);

            Response.Redirect(targetPage.ToString());
        }

        protected virtual int GetPublisherId(Publisher publisherFilter)
        {
            return BookManager.GetPublisherId(publisherFilter);
        }

        protected virtual void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            gridView.DataSource = publisherBookGroups;
            gridView.DataBind();
        }

        protected virtual BookCollection GetBookCollection()
        {
            return BookManager.GetBookCollection();
        }
    }
}