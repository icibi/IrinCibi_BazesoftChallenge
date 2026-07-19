using IrinCibiBlazesoftChallenge.Configuration;
using IrinCibiBlazesoftChallenge.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IrinCibiBlazesoftChallenge.Services
{
    public class MongoService
    {
            private readonly IMongoCollection<Players> _PlayersCollection;
            private readonly IMongoCollection<GameConfig> _configCollection;

        public MongoService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _PlayersCollection = database.GetCollection<Players>("Players");
            _configCollection = database.GetCollection<GameConfig>("GameConfig");
        }

        // Creates a new player in MongoDB.
        public async Task<Players> CreatePlayersAsync(Players Players)
            {
                await _PlayersCollection.InsertOneAsync(Players);
                return Players;
            }

        // Retrieves a player by ID.
        public async Task<Players> GetPlayersAsync(string id) =>
                await _PlayersCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        // Atomically increases or decreases a player's balance.
        public async Task<Players> AdjustBalanceAsync(string PlayersId, decimal amount)
            {
                var filter = Builders<Players>.Filter.Eq(p => p.Id, PlayersId);
                var update = Builders<Players>.Update.Inc(p => p.Balance, amount);

                var options = new FindOneAndUpdateOptions<Players>
                {
                    ReturnDocument = ReturnDocument.After
                };

                return await _PlayersCollection.FindOneAndUpdateAsync(filter, update, options);
            }

        // Deducts the bet only if the player has sufficient funds.
        public async Task<Players?> TryDeductBalanceAsync(string PlayersId, decimal betAmount)
            {
                var filter = Builders<Players>.Filter.And(
                    Builders<Players>.Filter.Eq(p => p.Id, PlayersId),
                    Builders<Players>.Filter.Gte(p => p.Balance, betAmount) // Balance must be >= bet
                );

                var update = Builders<Players>.Update.Inc(p => p.Balance, -betAmount);
                var options = new FindOneAndUpdateOptions<Players> { ReturnDocument = ReturnDocument.After };

                return await _PlayersCollection.FindOneAndUpdateAsync(filter, update, options);
            }

        // Retrieves the current slot machine dimensions from MongoDB.
        public async Task<GameConfig> GetGameConfigAsync()
            {
                var config = await _configCollection.Find(_ => true).FirstOrDefaultAsync();
                if (config == null)
                {
                    config = new GameConfig { Width = 5, Height = 3 }; // default starting size
                    await _configCollection.InsertOneAsync(config);
                }
                return config;
            }

        // Updates the slot machine dimensions without restarting the application.
        public async Task UpdateGameConfigAsync(int width, int height)
            {
                var config = await GetGameConfigAsync();
                var filter = Builders<GameConfig>.Filter.Eq(c => c.Id, config.Id);
                var update = Builders<GameConfig>.Update
                    .Set(c => c.Width, width)
                    .Set(c => c.Height, height);

                await _configCollection.UpdateOneAsync(filter, update);
            }
    }
 
}
