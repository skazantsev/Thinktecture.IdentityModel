using Chinook.Repository;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Owin.Authorization.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [ResourceAuthorize(ChinookResources.AlbumActions.View, ChinookResources.Album)]
    public class SearchController : Controller
    {
        IMusicRepository _musicRepository;

        public SearchController()
        {
            _musicRepository = new Chinook.Repository.InMemory.MusicRepository();
        }

        [Route("Search/{phrase}")]
        [Route("Search")]
        public ActionResult Search(string phrase)
        {
            var vm = new AlbumSearchViewModel(phrase, this._musicRepository);
            return View("Search", vm);
        }
	}
}