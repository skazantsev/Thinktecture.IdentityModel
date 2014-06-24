using Chinook.DomainModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chinook.Repository.InMemory
{
    class InMemoryCache
    {
        public static List<Album> Albums { get; private set; }
        public static List<AlbumCoverImage> AlbumCovers { get; private set; }
        public static List<Artist> Artists { get; private set; }
        public static List<Genre> Genres { get; private set; }
        public static List<Track> Tracks { get; private set; }
        

        static InMemoryCache()
        {
            LoadAllEntities();
        }

        private static void LoadAllEntities()
        {
            Albums = LoadEntities<Album>("albums.json");
            AlbumCovers = LoadEntities<AlbumCoverImage>("albumcovers.json");
            Artists = LoadEntities<Artist>("artists.json");
            Genres = LoadEntities<Genre>("genres.json");
            Tracks = LoadEntities<Track>("tracks.json");
        }

        private static List<T> LoadEntities<T>(string jsonFile)
            where T : class
        {
            var json = LoadJson(jsonFile);
            if (json == null) return new List<T>();

            var arr = Newtonsoft.Json.JsonConvert.DeserializeObject<T[]>(json);
            return arr.ToList();
        }

        const string prefix = "Chinook.Data.";

        private static string LoadJson(string jsonFile)
        {
            Assembly a = typeof(InMemoryCache).Assembly;
            using (var s = a.GetManifestResourceStream(prefix + jsonFile))
            {
                if (s != null)
                {
                    using (var sr = new StreamReader(s))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            return null;
        }
    }
}
