namespace LegacyCode.Bll
{
    public interface IBookRepository
    {
        int GetPublisherId(Publisher publisherFilter);
        BookCollection GetBookCollection();
    }

    public class BookRepository : IBookRepository
    {
        public int GetPublisherId(Publisher publisherFilter)
        {
            return BookManager.GetPublisherId(publisherFilter);
        }

        public BookCollection GetBookCollection()
        {
            return BookManager.GetBookCollection();
        }
    }
}