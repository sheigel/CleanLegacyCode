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

        private void ShowNoBooksPanel(string noBooksText)
        {
            errorLabel.Text = Resources.GetString(noBooksText);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayFilteredBooks();
        }

        public void DisplayFilteredBooks()
        {
            if (QueryStringPublisher == Publisher.Unknown)
            {
                //Display groups
                gridView.DataSource = BookManager.GetBookCollection().GetGroups;
                gridView.DataBind();
            }
            else
            {
                PublisherBookGroup publisherBookGroup = BookManager.GetBookCollection().GetPublisherGroup(QueryStringPublisher);

                if (publisherBookGroup != null)
                {
                    int bookCount = publisherBookGroup.Books.Count();

                    if (bookCount == 1)
                    {
                        // Show details.
                        Book book = publisherBookGroup.Books.First();

                        if (QueryStringBookClassification == Classification.Unknown || book.Classification == QueryStringBookClassification)
                        {
                            //Display book details
                            Uri targetPage = GetTargetPage(book.ISBN, book.Publisher);

                            Response.Redirect(targetPage.ToString());
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
                        gridView.DataSource = publisherGroups;
                        gridView.DataBind();
                    }
                }
                else
                {
                    //Display no books for publisher
                    ShowNoBooksPanel("We couldn't find any books matching your filter.");
                }
            }
        }
    }
}