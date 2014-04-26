using System.Collections.ObjectModel;
using LegacyCode.Bll;

namespace LegacyCode
{
    public interface IBookOverviewView
    {
        void DisplayError(string noBooksText);
        void DisplayBookDetails(Book book);
        void DisplayGroups(Collection<PublisherBookGroup> publisherBookGroups);
    }
}