
using System.Collections.Generic;
using System.Diagnostics;
using mroot_lib;
using System.Threading;

namespace Wox.Plugin.Dopamine
{
    static class Paths
    {
        public static string AudioDataPath => mroot.substitue_enviro_vars("||audio||");
        public static string DopamineExe => mroot.substitue_enviro_vars("||dopamine||");
    }

    public class Main : IPlugin, IReloadable
    {
        #region wox overrides
        public void Init(PluginInitContext context)
        {
            ReloadData();
        }
        public List<Result> Query(Query query)
        {
            List<Result> resultList = new List<Result>();
            this.AddCommands(resultList, query);
            return resultList;
        }
        public void ReloadData()
        {
            Catalog = AlbumLoader.Load(Paths.AudioDataPath);
        }

        #endregion

        #region commands

        private void PlayAlbum(Album album)
        {
            Process.Start("taskkill", "/F /IM Dopamine.exe");
            Thread.Sleep(3000);
            Process.Start(Paths.DopamineExe, $"{album.DirPath}");
        }
        private void AddPlayRandomAlbumCommand(List<Result> resultList, Query query)
        {
            if (StringTools.IsEqualOnStart(query.Search, "random", "rnd", "next"))
            {
                Result command = new Result
                {
                    Title = "Random album",
                    Score = 100,
                    IcoPath = "Images\\random.png",
                    Action = e =>
                    {
                        var album = Catalog.GetRandomAlbum();
                        if (album != null)
                        {
                            PlayAlbum(album);
                        }

                        return true;
                    }
                };
                resultList.Add(command);
            }
        }
        private void AddOpenAudioFolderCommand(List<Result> resultList, Query query)
        {
            if (StringTools.IsEqualOnStart(query.Search, "audio folder", "data", "music folder"))
            {
                Result command = new Result
                {
                    Title = "Open audio folder",
                    Score = 10,
                    IcoPath = "Images\\audio.png",
                    Action = e =>
                    {
                        ProcessStartInfo pinfo = new ProcessStartInfo
                        {
                            FileName = mroot.substitue_enviro_vars("||dcommander||"),
                            Arguments = $"-P L -T {Paths.AudioDataPath}"
                        };
                        Process.Start(pinfo);
                        return true;
                    }
                };
                resultList.Add(command);
            }
        }
        private void AddPlayAlbumCommand(List<Result> resultList, Album album)
        {
            Result commandResult = new Result
            {
                Title = album.FullName,
                SubTitle = album.DirPath,
                Score = 10,
                IcoPath = "Images\\album.png",
                Action = e =>
                {
                    PlayAlbum(album);
                    return true;
                }
            };
            resultList.Add(commandResult);
        }
        private void AddCommands(List<Result> resultList, Query query)
        {

            var albums = Catalog.GetAlbumsContains(query.Search);

            foreach (var album in albums)
            {
                AddPlayAlbumCommand(resultList, album);
            }

            AddOpenAudioFolderCommand(resultList, query);
            AddPlayRandomAlbumCommand(resultList, query);
        }

        #endregion

        #region members

        private AlbumCatalog Catalog { get; set; }

        #endregion
    }
}
