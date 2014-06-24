using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.ViewModels
{
    public class AlbumIndexViewModel
    {
        public int PageSize { get; set; }

        public AlbumIndexViewModel(Chinook.Repository.IMusicRepository musicRepository, int page, int pageSize = 5)
        {
            PageSize = pageSize;

            var albums = musicRepository.GetAlbums();
            int totalRows = albums.Count();
            int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);

            int currentPage = page;
            if (currentPage < 1) currentPage = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            int startRow = (currentPage - 1) * pageSize;

            CurrentPage = currentPage;
            TotalPages = totalPages;
            NextPage = currentPage < totalPages ? currentPage + 1 : (int?)null;
            PreviousPage = currentPage > 1 ? currentPage - 1 : (int?)null;
            StartingRow = startRow + 1;
            
            Albums =
                from a in albums.OrderBy(x => x.AlbumID).Skip(startRow).Take(pageSize)
                select new AlbumViewModel
                {
                    AlbumID = a.AlbumID,
                    Title = a.Title,
                    Artist = musicRepository.GetArtists().Where(x => x.ID == a.ArtistID).Select(x => x.Name).FirstOrDefault(),
                };
        }

        public IEnumerable<AlbumViewModel> Albums { get; set; }

        public int TotalPages { get; set; }
        public int StartingRow { get; set; }
        public int CurrentPage { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
    }
}