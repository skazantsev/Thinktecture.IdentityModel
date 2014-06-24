using Chinook.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApp.ViewModels
{
    public class AlbumEditViewModel
    {
        public AlbumEditViewModel(Chinook.DomainModel.Album album, Chinook.Repository.IMusicRepository musicRepository)
        {
            Album = new AlbumInputModel(album);
            Artists = new SelectList(musicRepository.GetArtists(), "ID", "Name", album.ArtistID);
        }
        
        public AlbumInputModel Album { get; set; }
        public IEnumerable<SelectListItem> Artists { get; set; }
    }

    public class AlbumInputModel
    {
        public AlbumInputModel()
        {
        }
        public AlbumInputModel(Album album)
        {
            this.AlbumID = album.AlbumID;
            this.Title = album.Title;
            this.ArtistID = album.ArtistID;
        }
        public Album GetAlbum()
        {
            return new Album
            {
                AlbumID = this.AlbumID,
                Title = this.Title,
                ArtistID = this.ArtistID,
            };
        }

        public virtual int AlbumID { get; set; }
        [Required]
        [StringLength(160)]
        [Remote("ValidateAlbumTitle", "Album", ErrorMessage="Album name already in use.", AdditionalFields="AlbumID")]
        public virtual string Title { get; set; }
        public virtual int ArtistID { get; set; }
    }
}