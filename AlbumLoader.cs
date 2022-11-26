using System.Collections.Generic;
using System.IO;

namespace Wox.Plugin.Dopamine
{
    static class AlbumLoader
    {
        #region api

        internal static AlbumCatalog Load(string rootFodlerPath)
        {
            AlbumCatalog catalog = new AlbumCatalog();

            if (Directory.Exists(rootFodlerPath) == false)
            {
                return catalog;
            }

            string[] dirs = Directory.GetDirectories(rootFodlerPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string dir in dirs)
            {
                catalog.Authors.Add(CreateTopFolder(dir));
            }

            return catalog;
        }

        #endregion

        #region private

        private static Author CreateTopFolder(string folderPath)
        {
            Author author = new Author();
            author.Name = Path.GetFileName(folderPath);
            author.Albums = new List<Album>();

            string[] dirs = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                Album song = new Album
                {
                    Name = Path.GetFileName(dir),
                    FullName = $"{author.Name} - {Path.GetFileName(dir)}",
                    DirPath = dir
                };

                author.Albums.Add(song);
            }

            return author;
        }

        #endregion
    }
}
