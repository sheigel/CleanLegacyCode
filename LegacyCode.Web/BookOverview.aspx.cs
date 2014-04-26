using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using LegacyCode.Bll;

namespace LegacyCode
{
    public partial class BookOverview : Page
    {
        private static Publisher QueryStringPublisher { get { throw new DependencyException(); } }

        private static Classification QueryStringBookClassification { get { throw new DependencyException(); } }

        private readonly IBookRepository bookRepository;

        public BookOverview() : this(new BookRepository())
        {
        }

        public BookOverview(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        private static Uri GetTargetPage(long bookId, Publisher publisher)
        {
            throw new DependencyException();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayFilteredBooks(QueryStringPublisher, QueryStringBookClassification);
        }

        public void DisplayFilteredBooks(Publisher publisherFilter, Classification classificationFilter)
        {
            new BookOverviewPresenter(bookRepository, this).DisplayFilteredBooks(publisherFilter, classificationFilter);
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