using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoDb.Web.Models.Configuration;
using NoDb.Web.Models.Cues;

namespace NoDb.Web.Data
{
    public class CuesMongoContext
    {
        private readonly IMongoDatabase _database;

        public CuesMongoContext(IOptions<AppOptions> settings)
        {
            var client = new MongoClient(settings.Value.MongoConfiguration.ConnectionString);
            _database = client.GetDatabase(settings.Value.MongoConfiguration.Database);
        }

        public IMongoCollection<CueMongo> Cues => _database.GetCollection<CueMongo>("cues");

        public IMongoCollection<CategoryMongo> Categories => _database.GetCollection<CategoryMongo>("cue_categories");
    }
}
