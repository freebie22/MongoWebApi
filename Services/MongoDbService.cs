using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoWebApi.Models;
using Newtonsoft.Json;

namespace MongoWebApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Playlist> _playlistCollection;

        public MongoDbService(IOptions<MongoDbSettings> mongoDBsettings)
        {
            MongoClient client = new MongoClient(mongoDBsettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBsettings.Value.DatabaseName);
            _playlistCollection = database.GetCollection<Playlist>(mongoDBsettings.Value.CollectionName);
        }

        public async Task CreateAsync(Playlist playlist)
        {
            await _playlistCollection.InsertOneAsync(playlist);
            return;
        }

        public async Task<List<Playlist>> GetAsync()
        {
           var playlist = await _playlistCollection.Find(new BsonDocument()).ToListAsync();
           var jsonPlaylist = JsonConvert.SerializeObject(playlist, Formatting.Indented);
           File.WriteAllText("Playlist.json", jsonPlaylist);
           return playlist;

        }

        public async Task AddToPlaylistAsync(string id, string movieId)
        {
            FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
            UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("items", movieId);
            await _playlistCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id)
        {
            FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
            await _playlistCollection.DeleteOneAsync(filter);
        }
    }
}
