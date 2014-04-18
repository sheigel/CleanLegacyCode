using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using LegayCode.Bll;

namespace LegayCode
{
    public interface IBookOverview
    {
        void ShowNoBooksPanel(string noBooksText);
        void DisplayBookDetails(Book book);
        void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups);
    }

    public partial class BookOverview : Page, IBookOverview
    {
        private static Publisher QueryStringPublisher { get { throw new DependencyException(); } }

        private static Classification QueryStringBookClassification { get { throw new DependencyException(); } }

        public virtual void ShowNoBooksPanel(string noBooksText)
        {
            errorLabel.Text = Resources.GetString(noBooksText);
        }

        public virtual void DisplayBookDetails(Book book)
        {
//Display book details
            Uri targetPage = GetTargetPage(book.ISBN, book.Publisher);

            Response.Redirect(targetPage.ToString());
        }

        public virtual void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
//Display groups
            gridView.DataSource = publisherBookGroups;
            gridView.DataBind();
        }


        private static Uri GetTargetPage(long bookId, Publisher publisher)
        {
            throw new DependencyException();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FilterBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public void FilterBooks(Publisher publisherQuery, Classification classificationQuery)
        {
            new BookPresenter(this, new BookRepository()).DisplayFilteredBooks(publisherQuery, classificationQuery);
        }
    }
}