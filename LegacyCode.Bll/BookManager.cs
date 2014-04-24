namespace LegacyCode.Bll
{
    public static class BookManager
    {
        public static BookCollection GetBookCollection()
        {
            throw new DependencyException();
        }

        public static int GetPublisherId(Publisher publisher)
        {
            throw new DependencyException();
        }

        public static Classification GetClassificationByGenre(Genre genre)
        {
            throw new DependencyException();
        }
    }
}