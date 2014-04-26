using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI;
using LegacyCode.Bll;

namespace LegacyCode
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
            DisplayFilteredBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public void DisplayFilteredBooks(Publisher publisherFilter, Classification classificationFilter)
        {
            if (publisherFilter == Publisher.Unknown)
            {
                //Display groups
                DisplayGroups(GetBookCollection().GetGroups);
            }
            else
            {
                int publisherId = GetPublisherId(publisherFilter);
                PublisherBookGroup publisherBookGroup = GetBookCollection().GetPublisherGroupById(publisherId);

                if (publisherBookGroup != null)
                {
                    int bookCount = publisherBookGroup.Books.Count();

                    if (bookCount == 1)
                    {
                        // Show details.
                        Book book = publisherBookGroup.Books.First();

                        if (classificationFilter == Classification.Unknown || book.Classification == classificationFilter)
                        {
                            //Display book details
                            DisplayBookDetails(book);
                        }
                        else
                        {
                            //Display no books for classification
                            ShowNoBooksPanel("We couldn't find any books matching your filter.");
                        }
                    }
                    else if (bookCount == 0)
                    {
                        //Display no books for publisher
                        ShowNoBooksPanel("We couldn't find any books matching your filter.");
                    }
                    else
                    {
                        var publisherGroups = new Collection<PublisherBookGroup> {publisherBookGroup};

                        //Display Groups
                        DisplayGroups(publisherGroups);
                    }
                }
                else
                {
                    //Display no books for publisher
                    ShowNoBooksPanel("We couldn't find any books matching your filter.");
                }
            }
        }

        protected virtual void DisplayBookDetails(Book book)
        {
            Uri targetPage = GetTargetPage(book.ISBN, book.Publisher);

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