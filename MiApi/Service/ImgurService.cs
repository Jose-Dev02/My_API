using System.Net.Http.Headers;


namespace MiApi.Service
{
    public class ImgurService
    {

        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _urlImgur;

        public ImgurService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
#pragma warning disable CS8601 // Possible null reference assignment.
            _clientId = configuration["Imgur:ClientId"];
            _urlImgur = configuration["Imgur:ImgurURL"];
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var requestContent = new MultipartFormDataContent();

            var imageContent = new StreamContent(imageStream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            requestContent.Add(imageContent, "image", fileName);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _clientId);

            var response = await _httpClient.PostAsync(_urlImgur, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error uploading image to Imgur: {responseContent}");
            }

            // Parse the response to get the image URL
            var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(responseContent);
            var imageUrl = jsonResponse["data"]["link"].ToString();
            return imageUrl;
        }

    }
}
