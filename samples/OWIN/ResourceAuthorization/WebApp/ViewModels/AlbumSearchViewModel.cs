using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.ViewModels
{
    public class AlbumSearchViewModel
    {
        public AlbumSearchViewModel(string phrase, Chinook.Repository.IMusicRepository musicRepository)
        {
            phrase = phrase.ToLower();

            var artists = musicRepository.GetArtists().ToArray();

            var artistQuery =
                from x in artists
                select x;
            if (!String.IsNullOrWhiteSpace(phrase))
            {
                artistQuery =
                    from x in artistQuery
                    where x.Name.ToLower().Contains(phrase)
                    select x;
            }

            var artistIDs =
                (from x in artistQuery
                 select x.ID).ToArray();

            var query =
                from x in musicRepository.GetAlbums()
                where artistIDs.Contains(x.ArtistID)
                select x;

            if (!String.IsNullOrWhiteSpace(phrase))
            {
                var query2 =
                    from x in musicRepository.GetAlbums()
                    where x.Title.ToLower().Contains(phrase)
                    select x;
                query = query.Union(query2);
            }

            var albums = query.Distinct();
            Albums =
                from a in albums
                select new AlbumViewModel
                {
                    AlbumID = a.AlbumID,
                    Title = a.Title,
                    Artist = artists.Where(x => x.ID == a.ArtistID).Select(x => x.Name).FirstOrDefault(),
                };
        }

        public IEnumerable<AlbumViewModel> Albums { get; set; }
    }
}