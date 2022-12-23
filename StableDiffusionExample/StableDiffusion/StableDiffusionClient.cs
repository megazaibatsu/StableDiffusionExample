using System.Text;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using StableDiffusionExample.Models;

namespace StableDiffusionExample.StableDiffusion
{

    public class StableDiffusionClient
    {
        public async Task<byte[]> TextToImage(string apiKey, string model, TextToImageOptions txt2img)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(txt2img), Encoding.UTF8, "application/json");
                var url = @"https://api.stability.ai/v1alpha/generation/" + model + @"/text-to-image";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "image/png" },
                        { HeaderNames.Authorization, apiKey }
                    },
                    Content = content
                };

                using (var httpClient = new HttpClient())
                {
                    using (var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
                    {
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                            byte[] data;
                            using (var memoryStream = new MemoryStream())
                            {
                                contentStream.CopyTo(memoryStream);
                                data = memoryStream.ToArray();
                            }

                            return data;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception thrown by TextToImage: {ex.Message}");
            }

            return null;
        }

        public async Task<byte[]> ImageToImage(string apiKey, string model, ImageToImageOptions img2img, byte[] image)
        {
            try
            {
                var optionsContent = new StringContent(JsonSerializer.Serialize(img2img), Encoding.UTF8, "application/json");
                var url = @"https://api.stability.ai/v1alpha/generation/" + model + @"/image-to-image";

                using (var buffer = new MemoryStream(image))
                {
                    var imageStreamContent = new StreamContent(buffer);

                    var formContent = new MultipartFormDataContent
                    {
                        { optionsContent, "options" },
                        { imageStreamContent, "init_image" }
                    };

                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Headers =
                        {
                            { HeaderNames.Accept, "image/png" },
                            { HeaderNames.Authorization, apiKey }
                        },
                        Content = formContent
                    };

                    using (var httpClient = new HttpClient())
                    {
                        using (var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
                        {
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                                byte[] data;
                                using (var memoryStream = new MemoryStream())
                                {
                                    contentStream.CopyTo(memoryStream);
                                    data = memoryStream.ToArray();
                                }

                                return data;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown by ImageToImage: {ex.Message}");
            }

            return null;
        }

        // See: https://github.com/Stability-AI/stability-sdk/issues/70
        public static decimal CalculateStartSchedule(decimal imageStrength)
        {
            if (imageStrength < 0m)
                imageStrength = 0m;

            if (imageStrength > 0.95m)
                imageStrength = 0.95m;

            var startSchedule = 1 - imageStrength;
            return startSchedule;
        }
    }
}
