using Blog_Api;
using DataLayer.Model_Blog;
using DataLayer.Model_Hatalar;
using DataLayer.Model_Parcala;
using DataLayer.Model_VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Odev_v9.Controllers
{
    public class YazarController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public YazarController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Makaleler()
        {
            string token = HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = "https://localhost:7052/api/";
            string apiEndpoint = "Yazar/basliklar";
            string url = string.Concat(baseUrl, apiEndpoint);
            if (token == null)
            {
                return BadRequest("Token yok olm nereye gidiyorsun");
            }

            string jsonPayload = Base64UrlHelper.Base64UrlDecode(token.Split('.')[1]);
            var payload = JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
            string yazarid = (payload.Name);
            //var serializeUser = JsonConvert.SerializeObject(yazarid);

            // API'ye veri gönderme
            Sorgu sor = new Sorgu()
            {
                Name = yazarid
            };

            var payloaad = sor;
            //string yazarid = payloaad.Name;
            
            var content = new StringContent(JsonConvert.SerializeObject(payloaad), Encoding.UTF8, "application/json");


            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                else
                {
                    // API hatalarını işleme
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException ex)
            {
                // İstek hatalarını işleme
                return StatusCode(StatusCodes.Status500InternalServerError, $"Request error: {ex.Message}");
            }
            
        }

        [HttpGet]
        public IActionResult MakaleYaz()
        {
            string token = HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Yaz(MakaleViewModel model)
        {
            // Token'ı al
            string token = HttpContext.Session.GetString("Token");

            // Token yoksa hata döndür
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token mevcut değil.");
            }

            // HttpClient oluştur ve Authorization header'ını ayarla
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = "https://localhost:7052/api/";
            string apiEndpoint = "Yazar/makale-ekle";
            string url = $"{baseUrl}{apiEndpoint}";

            string jsonPayload = Base64UrlHelper.Base64UrlDecode(token.Split('.')[1]);
            var payload = JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
            string yazarid = payload.Name;
            model.Yaz=yazarid;
            // Model verilerini JSON formatına dönüştür
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            try
            {
                // API'ye POST isteği gönder
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Yanıtı kontrol et ve uygun şekilde işle
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return Ok(responseContent);
                }
                else
                {
                    // API hatalarını işleme
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // İstek hatalarını işleme
                return StatusCode(StatusCodes.Status500InternalServerError, $"İstek hatası: {ex.Message}");
            }
        }
        //https://localhost:7052/api/Yazar/makale-ekle

    }
}




