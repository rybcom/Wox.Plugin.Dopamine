using mroot_lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wox.Plugin.Dopamine
{
    public class AlbumCatalog
    {
        public AlbumCatalog()
        {
            Authors = new List<Author>();
        }

        public List<Author> Authors { get; set; }

        public IEnumerable<Album> GetAlbums()
        {
            return Authors.SelectMany(x => x.Albums);
        }

        public IEnumerable<Album> GetAlbumsStartWith(string text)
        {
            var songByName = GetAlbums().Where(x => StringTools.IsEqualOnStart(x.Name, text));
            var songByAuthor = Authors.Where(x => StringTools.IsEqualOnStart(x.Name, text)).SelectMany(x => x.Albums);

            return songByName.Union(songByAuthor);
        }

        public Album GetRandomAlbum()
        {
            return GetAlbums().OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        public IEnumerable<Album> GetAlbumsContains(string text)
        {
            var songByName = GetAlbums().Where(x => StringTools.ContainsIgnoreCase(x.Name, text));
            var songByAuthor = Authors.Where(x => StringTools.ContainsIgnoreCase(x.Name, text)).SelectMany(x => x.Albums);

            return songByName.Union(songByAuthor);
        }

    }

    public class Author
    {
        public string Name { get; set; }
        public List<Album> Albums { get; set; }
    }

    public class Album
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DirPath { get; set; }
    }
}
