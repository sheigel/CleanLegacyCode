﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace LegayCode.Bll
{
    [Serializable]
    public class BookCollection : Collection<Book>
    {
        public BookCollection()
        {
        }

        public BookCollection(XmlNodeList bookNodes)
        {
            throw new DependencyException();
        }

        public Collection<PublisherBookGroup> GetGroups
        {
            get
            {
                var groups = new Collection<PublisherBookGroup>();

                var groupNames = new Dictionary<int, string>();

                foreach (Book book in Items)
                {
                    if (!groupNames.ContainsKey(book.PublisherId))
                    {
                        groupNames.Add(book.PublisherId, book.PublisherDescription);
                    }
                }

                foreach (var group in groupNames)
                {
                    groups.Add(
                        new PublisherBookGroup(
                            GetBookCollectionPublisherGroupId(group.Key),
                            group.Key,
                            group.Value));
                }

                return groups;
            }
        }

        public BookCollection GetBookCollectionPublisherGroupId(int publisherId)
        {
            var bookCollection = new BookCollection();
            foreach (Book book in
                Items.Where(b => b.PublisherId == publisherId))
            {
                bookCollection.Add(book);
            }

            return bookCollection;
        }

        public PublisherBookGroup GetPublisherGroup(Publisher publisher)
        {
            int publisherId = BookManager.GetPublisherId(publisher);
            if (publisherId > 0)
            {
                PublisherBookGroup group = GetGroups.FirstOrDefault(p => p.PublisherGroupId == publisherId);
                if (group != null)
				{
					return group;
				}
				else 
				{
					return new PublisherBookGroup(new BookCollection(), publisherId, string.Empty);
				}
            }
            else
            {
                return null;
            }
        }

        public void Remove(Predicate<Book> predicate)
        {
            List<Book> itemsToRemove = Items.Where(book => predicate(book)).ToList();
            foreach (Book book in itemsToRemove)
            {
                Remove(book);
            }
        }
    }
}