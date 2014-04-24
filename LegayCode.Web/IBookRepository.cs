using System.Collections.ObjectModel;
using LegayCode.Bll;

namespace LegayCode
{
    public interface IBookRepository
    {
        PublisherBookGroup GetPublisherBookGroup(Publisher publisherQuery);
        BookCollection GetBookCollection();
    }
}