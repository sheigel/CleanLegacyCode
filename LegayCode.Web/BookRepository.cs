using System.Collections.ObjectModel;
using System.Linq;
using LegayCode;
using LegayCode.Bll;

public  class BookRepository :IBookRepository
{
    public virtual PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery)
    {
        return BookManager.GetBookCollection().GetPublisherGroup(publisherQuery);
    }

    public virtual BookCollection GetBookCollection()
    {
        return BookManager.GetBookCollection();
    }
}