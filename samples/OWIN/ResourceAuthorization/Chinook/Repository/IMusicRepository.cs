using Chinook.DomainModel;
using System.Linq;

namespace Chinook.Repository
{
    public interface IMusicRepository
    {
        IQueryable<Album> GetAlbums();
        Album GetAlbum(int id);
        void SaveAlbum(Album album);

        IQueryable<Artist> GetArtists();
        Artist GetArtist(int id);

        AlbumCover GetAlbumCoverByAlbumID(int albumID);
        AlbumCoverImage GetAlbumCoverImageByAlbumID(int albumID);
        void SaveAlbumCoverImage(AlbumCoverImage img);

        IQueryable<Genre> GetGenres();
        Genre GetGenre(int genreID);
        void SaveGenre(Genre genre);

        IQueryable<Track> GetTracks();
        IQueryable<Track> GetTracks(int albumID);
        Track GetTrack(int trackID);
        void SaveTrack(Track track);
    }
}
