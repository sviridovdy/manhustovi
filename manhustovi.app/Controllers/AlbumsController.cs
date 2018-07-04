using manhustovi.app.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace manhustovi.app.Controllers
{
    [Route("api")]
    public class AlbumsController : Controller
    {
        private readonly AlbumsRepository albumsRepository;

        public AlbumsController(AlbumsRepository albumsRepository)
        {
            this.albumsRepository = albumsRepository;
        }

        [Route("albums/{id}")]
        public ActionResult GetById([FromRoute] string id)
        {
            var album = albumsRepository.GetAlbum(id);
            return new JsonResult(album);
        }
    }
}