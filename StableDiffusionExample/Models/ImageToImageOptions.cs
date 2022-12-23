using System.Text.Json.Serialization;

namespace StableDiffusionExample.Models
{
    public class ImageToImageOptions
    {
        [JsonPropertyName("cfg_scale")]
        public int CfgScale { get; set; } = 7;

        [JsonPropertyName("clip_guidance_preset")]
        public string ClipGuidancePreset { get; set; } = "FAST_BLUE";

        [JsonPropertyName("height")]
        public int Height { get; set; } = 512;

        [JsonPropertyName("width")]
        public int Width { get; set; } = 512;

        [JsonPropertyName("sampler")]
        public string Sampler { get; set; } = "K_DPM_2_ANCESTRAL";

        [JsonPropertyName("samples")]
        public int Samples { get; set; } = 1;

        [JsonPropertyName("seed")]
        public int Seed { get; set; } = 42;

        [JsonPropertyName("steps")]
        public int Steps { get; set; } = 75;

        [JsonPropertyName("step_schedule_start")]
        public decimal StepSheduleStart { get; set; } = 0.06m;

        [JsonPropertyName("step_schedule_end")]
        public decimal StepSheduleEnd { get; set; } = 0.05m;

        [JsonPropertyName("text_prompts")]
        public List<TextPrompt> TextPrompts { get; set; } = new List<TextPrompt>();
    }
}
