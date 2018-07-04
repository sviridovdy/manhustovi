using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using manhustovi.app.Models;
using manhustovi.app.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace manhustovi.app.Controllers
{
    [Route("api")]
    public class PostsController : Controller
    {
        private readonly PostsRepository postsRepository;

        private readonly AlbumsRepository albumsRepository;

        public PostsController(PostsRepository postsRepository, AlbumsRepository albumsRepository)
        {
            this.postsRepository = postsRepository;
            this.albumsRepository = albumsRepository;
        }

        [Route("post/{id}")]
        public ActionResult GetPost([FromRoute] string id)
        {
            try
            {
                var posts = postsRepository.GetPost(id)
                    .Select(p => new PostDto(p, albumsRepository.GetAlbumAttachment(p.AlbumId))).ToArray();
                var response = new PostsResponse(posts, postsRepository.PostsCount);
                return new JsonResult(response);
            }
            catch (Exception x)
            {
                return new ContentResult
                {
                    Content = x.ToString(),
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    ContentType = "text/plain"
                };
            }
        }

        [Route("posts")]
        public ActionResult GetPosts([FromQuery] int offset, [FromQuery] int count)
        {
            try
            {
                var posts = postsRepository.GetPosts(offset, count)
                    .Select(p => new PostDto(p, albumsRepository.GetAlbumAttachment(p.AlbumId))).ToArray();
                var response = new PostsResponse(posts, postsRepository.PostsCount);
                return new JsonResult(response);
            }
            catch (Exception x)
            {
                return new ContentResult
                {
                    Content = x.ToString(),
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    ContentType = "text/plain"
                };
            }
        }

        [Route("update")]
        public async Task<ActionResult> Update()
        {
            await postsRepository.Update();
            albumsRepository.LoadData();
            return new OkResult();
        }
    }

    public class PostsResponse
    {
        public PostDto[] Posts { get; }

        public int PostsCount { get; }

        public PostsResponse(PostDto[] posts, int postsCount)
        {
            Posts = posts;
            PostsCount = postsCount;
        }
    }
}