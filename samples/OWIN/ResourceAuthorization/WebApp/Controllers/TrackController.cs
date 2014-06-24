using Chinook.DomainModel;
using Chinook.Repository;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Owin.Authorization.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ResourceAuthorize(ChinookResources.AlbumActions.Edit, ChinookResources.Track)]
    public class TrackController : Controller
    {
        IMusicRepository _musicRepository;

        public TrackController()
        {
            _musicRepository = new Chinook.Repository.InMemory.MusicRepository();
        }

        public ActionResult List(int albumID)
        {
            var tracks = _musicRepository.GetTracks(albumID);
            return PartialView(tracks);
        }

        public ActionResult Edit(int id)
        {
            var track = _musicRepository.GetTrack(id);
            if (track == null) return HttpNotFound();
            var vm = new TrackEditViewModel(_musicRepository, track);
            return View("Edit", vm);
        }

        [HttpPost]
        public ActionResult Update(Track track)
        {
            if (ModelState.IsValid)
            {
                _musicRepository.SaveTrack(track);
                return RedirectToAction("Edit", "Album", new { id = track.AlbumID });
            }
            else
            {
                var vm = new TrackEditViewModel(_musicRepository, track);
                return View("Edit", vm);
            }
        }

	}
}