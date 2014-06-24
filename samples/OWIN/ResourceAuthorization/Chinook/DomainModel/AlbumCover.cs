using System.ComponentModel.DataAnnotations;

namespace Chinook.DomainModel
{
    public class AlbumCover
    {
        [Key]
        public virtual int AlbumID { get; set; }
        public virtual string ImageUrl { get; set; }
    }

    public class AlbumCoverImage : AlbumCover
    {
        public virtual byte[] Image { get; set; }
    }
}
