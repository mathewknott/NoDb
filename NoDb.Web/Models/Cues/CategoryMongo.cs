using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoDb.Web.Models.Cues
{
    public class CategoryMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DisplayName("Category Number")]
        public int CategoryNumber { get; set; } = 0;
    }
 }