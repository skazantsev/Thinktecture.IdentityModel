using Chinook.DomainModel;
using Chinook.Repository;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TrackEditViewModel
    {
        public TrackEditViewModel(IMusicRepository musicRepository, Track track)
        {
            Track = track;
            Genres = new SelectList(musicRepository.GetGenres(), "ID", "Name");
            Albums = new SelectList(musicRepository.GetAlbums(), "AlbumID", "Title");
        }

        public Track Track { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
        public IEnumerable<SelectListItem> Albums { get; set; }
    }
}
