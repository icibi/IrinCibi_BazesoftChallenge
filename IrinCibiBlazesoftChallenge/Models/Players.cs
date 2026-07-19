 using MongoDB.Bson;
 using MongoDB.Bson.Serialization.Attributes;

    namespace IrinCibiBlazesoftChallenge.Models
    {
    //Represents a player stored in MongoDB.
        public class Players
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? Id { get; set; }

            [BsonElement("username")]
            public string Username { get; set; } = null!;

            [BsonElement("balance")]
            public decimal Balance { get; set; }
        }
    }