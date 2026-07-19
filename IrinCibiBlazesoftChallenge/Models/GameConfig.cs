using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IrinCibiBlazesoftChallenge.Models;

//Stores the configurable slot machine dimensions.
public class GameConfig
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("width")]
    public int Width { get; set; }

    [BsonElement("height")]
    public int Height { get; set; }
}