using System;
using System.Xml;

namespace LegacyCode.Bll
{
    [Serializable]
    public class Book
    {
        public Book(XmlNode bookNode)
        {
            throw new DependencyException();
        }

        public int PublisherId { get; private set; }

        public string PublisherDescription { get; private set; }

        public AuthorCollection Authors { get; private set; }

        public Genre Genre
        {
            get { throw new DependencyException(); }
        }

        public long ISBN { get; private set; }

        public Publisher Publisher
        {
            get { throw new DependencyException(); }
        }

        public Classification Classification    { get; set; }
    }
}