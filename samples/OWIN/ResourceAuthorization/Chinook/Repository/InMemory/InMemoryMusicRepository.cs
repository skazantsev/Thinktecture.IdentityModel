using Chinook.DomainModel;
using System;
using System.Linq;

namespace Chinook.Repository.InMemory
{
    public class MusicRepository : IMusicRepository
    {
        public IQueryable<Album> GetAlbums()
        {
            return InMemoryCache.Albums.AsQueryable();
        }

        public Album GetAlbum(int id)
        {
            var q = from x in GetAlbums()
                    where x.AlbumID == id
                    select x;
            return q.SingleOrDefault();
        }

        public void SaveAlbum(Album album)
        {
            if (album == null) throw new ArgumentNullException("album");

            var q = from x in GetAlbums()
                    where x.AlbumID == album.AlbumID
                    select x;
            var dbItem = q.FirstOrDefault();
            if (dbItem == null)
            {
                dbItem = new Album();
                InMemoryCache.Albums.Add(dbItem);
            }

            dbItem.Title = album.Title;
            dbItem.ArtistID = album.ArtistID;
        }

        public IQueryable<Artist> GetArtists()
        {
            return InMemoryCache.Artists.AsQueryable();
        }

        public Artist GetArtist(int id)
        {
            var q = from x in GetArtists()
                    where x.ID == id
                    select x;
            return q.SingleOrDefault();
        }

        public AlbumCover GetAlbumCoverByAlbumID(int albumID)
        {
            var q = from x in InMemoryCache.AlbumCovers
                    where x.AlbumID == albumID
                    select new AlbumCover
                    {
                        AlbumID = x.AlbumID,
                        ImageUrl = x.ImageUrl
                    };
            return q.FirstOrDefault();
        }

        public AlbumCoverImage GetAlbumCoverImageByAlbumID(int albumID)
        {
            var q = from x in InMemoryCache.AlbumCovers
                    where x.AlbumID == albumID
                    select x;
            return q.FirstOrDefault();
        }

        public void SaveAlbumCoverImage(AlbumCoverImage img)
        {
            if (img == null) throw new ArgumentNullException("img");

            var q = from x in InMemoryCache.AlbumCovers
                    where x.AlbumID == img.AlbumID
                    select x;
            var dbItem = q.FirstOrDefault();
            if (dbItem == null)
            {
                dbItem = new AlbumCoverImage();
                InMemoryCache.AlbumCovers.Add(dbItem);
            }
            dbItem.AlbumID = img.AlbumID;
            dbItem.ImageUrl = img.ImageUrl;
            dbItem.Image = img.Image;
        }

        public IQueryable<Genre> GetGenres()
        {
            return InMemoryCache.Genres.AsQueryable();
        }

        public Genre GetGenre(int genreID)
        {
            var q = from x in GetGenres()
                    where x.ID == genreID
                    select x;
            return q.FirstOrDefault();
        }

        public void SaveGenre(Genre genre)
        {
            if (genre == null) throw new ArgumentNullException("genre");

            var q = from x in GetGenres()
                    where x.ID == genre.ID
                    select x;
            var dbItem = q.FirstOrDefault();
            if (dbItem == null)
            {
                dbItem = new Genre();
                InMemoryCache.Genres.Add(dbItem);
            }

            dbItem.Name = genre.Name;
        }

        public IQueryable<Track> GetTracks()
        {
            return InMemoryCache.Tracks.AsQueryable();
        }

        public IQueryable<Track> GetTracks(int albumID)
        {
            var q = from x in GetTracks()
                    where x.AlbumID == albumID
                    select x;
            return q;
        }

        public Track GetTrack(int trackID)
        {
            var q = from x in GetTracks()
                    where x.ID == trackID
                    select x;
            return q.FirstOrDefault();
        }

        public void SaveTrack(Track track)
        {
            if (track == null) throw new ArgumentNullException("track");

            var q = from x in GetTracks()
                    where x.ID == track.ID
                    select x;
            var dbItem = q.FirstOrDefault();
            if (dbItem == null)
            {
                dbItem = new Track();
                InMemoryCache.Tracks.Add(dbItem);
            }
            dbItem.Name = track.Name;
            dbItem.Price = track.Price;
            dbItem.AlbumID = track.AlbumID;
            dbItem.GenreID = track.GenreID;
        }
    }
}
