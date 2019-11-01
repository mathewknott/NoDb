using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoDb.Web.Models.Cues
{
    public class CueMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string CueId { get; set; } = string.Empty;

        public int Sequence { get; set; } = 0;

        public string Question { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;

        public CategoryMongo Category { get; set; }

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime Created { get; set; } = DateTime.Now;

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}