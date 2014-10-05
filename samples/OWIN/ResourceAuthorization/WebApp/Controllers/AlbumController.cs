using Chinook.Repository;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [ResourceAuthorize(ChinookResources.AlbumActions.View, ChinookResources.Album)]
    public class AlbumController : Controller
    {
        IMusicRepository _musicRepository;

        public AlbumController()
        {
            _musicRepository = new Chinook.Repository.InMemory.MusicRepository();
        }

        public ActionResult Index(int page = 1)
        {
            var vm = new AlbumIndexViewModel(this._musicRepository, page);
            return View("Index", vm);
        }

        public ActionResult Edit(int id)
        {
            if (!HttpContext.CheckAccess(
                ChinookResources.AlbumActions.Edit,
                ChinookResources.Album,
                id.ToString()))
            {
                return this.AccessDenied();
            }

            var album = _musicRepository.GetAlbum(id);
            if (album == null) return RedirectToAction("Index");

            var vm = new AlbumEditViewModel(album, _musicRepository);
            return View("Edit", vm);
        }

        [HttpPost]
        public ActionResult Update(AlbumInputModel album)
        {
            if (!HttpContext.CheckAccess(
                ChinookResources.AlbumActions.Edit, 
                ChinookResources.Album, 
                album.AlbumID.ToString()))
            {
                return this.AccessDenied();
            }

            _musicRepository.SaveAlbum(album.GetAlbum());

            return RedirectToAction("Index");
        }

        public ActionResult AlbumCover(int id)
        {
            var image = _musicRepository.GetAlbumCoverImageByAlbumID(id);
            if (image != null) return File(image.Image, "image/jpeg");
            return File("~/Content/question-mark.png", "image/png");
        }

        public ActionResult ValidateAlbumTitle(
            [Bind(Prefix = "Album.AlbumID")] int albumID,
            [Bind(Prefix = "Album.Title")] string title)
        {
            var query =
                from a in _musicRepository.GetAlbums()
                where a.Title.ToLower() == title.ToLower() && a.AlbumID != albumID
                select a;
            var exists = query.Any();
            return Json(!exists, JsonRequestBehavior.AllowGet);
        }
    }
}
