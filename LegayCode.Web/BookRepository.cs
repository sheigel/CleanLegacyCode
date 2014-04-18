using System.Collections.ObjectModel;
using LegayCode;
using LegayCode.Bll;

public  class BookRepository :IBookRepository
{
    public PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery)
    {
        return BookManager.GetBookCollection().GetPublisherGroup(publisherQuery);
    }

    public Collection<PublisherBookGroup> GetPublisherBookGroups()
    {
        return BookManager.GetBookCollection().GetGroups;
    }
}