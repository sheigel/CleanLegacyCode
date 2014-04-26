using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using LegacyCode.Bll;

namespace LegacyCode
{
    public partial class BookOverview : Page, IBookOverviewView
    {
        private static Publisher QueryStringPublisher { get { throw new DependencyException(); } }

        private static Classification QueryStringBookClassification { get { throw new DependencyException(); } }

        private readonly BookOverviewPresenter presenter;

        public BookOverview()
        {
            presenter = new BookOverviewPresenter(new BookRepository(), this);
        }

        private static Uri GetTargetPage(long bookId, Publisher publisher)
        {
            throw new DependencyException();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter.DisplayFilteredBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public virtual void DisplayError(string noBooksText)
        {
            errorLabel.Text = Resources.GetString(noBooksText);
        }

        public virtual void DisplayBookDetails(Book book)
        {
            var targetPage = GetTargetPage(book.ISBN, book.Publisher);

            Response.Redirect(targetPage.ToString());
        }

        public virtual void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups)
        {
            gridView.DataSource = publisherBookGroups;
            gridView.DataBind();
        }
    }
}