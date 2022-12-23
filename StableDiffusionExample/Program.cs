using StableDiffusionExample.Models;
using StableDiffusionExample.StableDiffusion;

// This example demonstrates the text-to-image and image-to-image using the Stable Diffusion REST API.
// See the docs at: https://api.stability.ai/docs#tag/v1alphageneration/

var apiKey = "PUT YOUR API KEY HERE";
var model = "stable-diffusion-512-v2-0";

var stableDiffusion = new StableDiffusionClient();

// Text-to-Image
var txt2img = new TextToImageOptions
{
    Width = 512,
    Height = 512,
    CfgScale = 7,
    Steps = 40,
    Samples = 1,
    Sampler = "K_DPM_2_ANCESTRAL",
    ClipGuidancePreset = "FAST_BLUE",
    Seed = 42,
    TextPrompts = new List<TextPrompt>
    {
        new TextPrompt
        {
            Text = "A horse riding an astronaut",
            Weight = 1
        }
    },
};

var txt2img_file = await stableDiffusion.TextToImage(apiKey, model, txt2img);

if (txt2img_file is not null && txt2img_file.Length > 0)
{
    var filename = txt2img.TextPrompts[0].Text.Replace(' ', '_');
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{filename}.png");
    File.WriteAllBytes(filePath, txt2img_file);
}

// Image-to-Image
var imageStrength = 0.4m;

var img2img = new ImageToImageOptions
{
    Width = 512,
    Height = 512,
    CfgScale = 7,
    Steps = 40,
    Samples = 1,
    Sampler = "K_DPM_2_ANCESTRAL",
    ClipGuidancePreset = "FAST_BLUE",
    Seed = 42,
    TextPrompts = new List<TextPrompt>
    {
        new TextPrompt
        {
            Text = "A horse riding an unicycle",
            Weight = 1
        }
    },
    StepSheduleStart = StableDiffusionClient.CalculateStartSchedule(imageStrength),
    StepSheduleEnd = 0.05m
};

var initImagePath = Path.Combine(Directory.GetCurrentDirectory(), $"../../../init_image.png");
var initImage = File.ReadAllBytes(initImagePath);
var img2img_file = await stableDiffusion.ImageToImage(apiKey, model, img2img, initImage);

if (img2img_file is not null && img2img_file.Length > 0)
{
    var filename = img2img.TextPrompts[0].Text.Replace(' ', '_');
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{filename}.png");
    File.WriteAllBytes(filePath, img2img_file);
}
