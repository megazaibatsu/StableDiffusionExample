using System.Text.Json.Serialization;

namespace StableDiffusionExample.Models
{
    public class TextPrompt
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; } = 1;
    }
}