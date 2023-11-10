using Microsoft.AspNetCore.Mvc;
using MongoWebApi.Models;
using MongoWebApi.Services;

namespace MongoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly MongoDbService _mongoDBService;

        public PlaylistController(MongoDbService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Playlist>> GetPlaylists()
        {
            return await _mongoDBService.GetAsync();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist playlist)
        {
            await _mongoDBService.CreateAsync(playlist);
            return CreatedAtRoute(nameof(GetPlaylists), new { id = playlist.Id }, playlist);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddToPlaylist(string id, [FromBody] string movieId)
        {
            await _mongoDBService.AddToPlaylistAsync(id, movieId);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveFromPlaylist(string id)
        {
            await _mongoDBService.DeleteAsync(id);
            return NoContent();
        }
    }
}
