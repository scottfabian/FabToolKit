using System.Text.Json.Serialization;

namespace FabToolKit.Tkx.Sentinel.Rest;

public class PostJobResponseDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("creationDate")]
    public DateTime CreationDate { get; set; }

    [JsonPropertyName("processingDate")]
    public DateTime ProcessingDate { get; set; }

    [JsonPropertyName("resultMessage")]
    public string ResultMessage { get; set; }
}

