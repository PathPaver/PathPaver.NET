using MongoDB.Bson.Serialization.Attributes;
using PathPaver.Domain.Common;

public class Graph : BaseEntity
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("labels")]
    public List<string> Labels { get; set; } = new List<string>();

    [BsonElement("datasets")]
    public List<Dataset> Datasets { get; set; } = new List<Dataset>();

    public Graph() {}

    public Graph(string name, List<string> labels, List<Dataset> datasets)
    {
        Name = name;
        Labels = labels;
        Datasets = datasets;
    }
}

public class Dataset
{
    [BsonElement("label")]
    public string Label { get; set; } = string.Empty;

    [BsonElement("data")]
    public List<double> Data { get; set; } = new List<double>();
}