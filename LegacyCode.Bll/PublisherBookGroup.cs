using System;
using System.Linq;

namespace LegacyCode.Bll
{
    [Serializable]
    public class PublisherBookGroup
    {
        public PublisherBookGroup(BookCollection books, int publisherGroupId, string publisherGroupName)
        {
            PublisherGroupId = publisherGroupId;
            PublisherGroupName = publisherGroupName;
            Books = books;
            SetAuthors();
        }

        public AuthorCollection Authors { get; private set; }

        public int PublisherGroupId { get; set; }

        public string PublisherGroupName { get; set; }

        public BookCollection Books { get; private set; }

        private void SetAuthors()
        {
            var authors = new AuthorCollection();

            foreach (Author account in Books.SelectMany(book => book.Authors))
            {
                authors.Add(account);
            }

            Authors = authors;
        }
    }
}