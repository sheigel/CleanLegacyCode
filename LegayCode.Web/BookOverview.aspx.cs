using System;
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

        private static Uri GetTargetPage(
            long bookId,
            Publisher publisher)
        {
            throw new DependencyException();
        }

        private void ShowNoBooksPanel(string noBooksText)
        {
            errorLabel.Text = Resources.GetString(noBooksText);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (QueryStringPublisher == Publisher.Unknown)
            {
                gridView.DataSource = BookManager.GetBookCollection().GetGroups;
                gridView.DataBind();
            }
            else
            {
                PublisherBookGroup publisherBookGroup =
                    BookManager.GetBookCollection()
                        .GetPublisherGroup(QueryStringPublisher);

                if (publisherBookGroup != null)
                {
                    int bookCount = publisherBookGroup.Books.Count();

                    if (bookCount == 1)
                    {
                        // Show details.
                        Book book =
                            publisherBookGroup.Books.First();

                        if (QueryStringBookClassification == Classification.Unknown
                            || BookManager.GetClassificationByGenre(
                                book.Genre)
                            == QueryStringBookClassification)
                        {
                            Uri targetPage = GetTargetPage(
                                book.ISBN,
                                book.Publisher);

                            Response.Redirect(targetPage.ToString());
                        }
                        else
                        {
                            ShowNoBooksPanel(string.Format("No books for the classification {0} available.",
                                                           QueryStringBookClassification));
                        }
                    }
                    else if (bookCount == 0)
                    {
                        ShowNoBooksPanel(string.Format(
                            "No books for the {0} publisher available.", QueryStringPublisher));
                    }
                    else
                    {
                        var publisherGroups =
                            new Collection<PublisherBookGroup> {publisherBookGroup};

                        gridView.DataSource = publisherGroups;
                        gridView.DataBind();
                    }
                }
                else
                {
                    ShowNoBooksPanel(string.Format(
                        "No books for the {0} publisher available.", QueryStringPublisher));
                }
            }
        }
    }
}