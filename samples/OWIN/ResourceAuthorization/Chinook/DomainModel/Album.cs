using System.ComponentModel.DataAnnotations;

namespace Chinook.DomainModel
{
    public class Album
    {
        [Key]
        public virtual int AlbumID { get; set; }
        [Required]
        [StringLength(160)]
        public virtual string Title { get; set; }
        public virtual int ArtistID { get; set; }
    }
}
