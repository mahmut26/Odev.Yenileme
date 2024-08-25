using DataLayer.Model_VM;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using DataLayer.Model_Parcala;
using Newtonsoft.Json;
using System.Text;

namespace Odev_v9.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public KullaniciController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult KategoriEkle()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Ekle(Link link)
        {
            string token = HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = "https://localhost:7052/api/";
            string metot = "Kullanici/kategori-ekle";

            

            
            string url = string.Concat(baseUrl, metot);
            if (token == null)
            {
                return BadRequest("Token yok olm nereye gidiyorsun");
            }
            string jsonPayload = Base64UrlHelper.Base64UrlDecode(token.Split('.')[1]);
            var payload = JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
            string yazarid = (payload.Name); //name'i aldı

            link.sorgu1= yazarid;

            var content = new StringContent(JsonConvert.SerializeObject(link), Encoding.UTF8, "application/json");


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
        //[HttpGet]
        //public IActionResult Baslik()
        //{

        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> Baslik()
        {
            string token = HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = "https://localhost:7052/api/";
            string metot = "Kullanici/baslik-goster";




            string url = string.Concat(baseUrl, metot);
            if (token == null)
            {
                return BadRequest("Token yok olm nereye gidiyorsun");
            }
            string jsonPayload = Base64UrlHelper.Base64UrlDecode(token.Split('.')[1]);
            var payload = JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
            string yazarid = (payload.Name); //name'i aldı

            Sorgu sor = new Sorgu()
            {
                Name = yazarid
            };

            var content = new StringContent(JsonConvert.SerializeObject(sor), Encoding.UTF8, "application/json");


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
        public async Task<IActionResult> Icerik(Sorgu sorgu) //Burada MVC döndürecek. MVC YOK !!!
        {
            string token = HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = "https://localhost:7052/api/";
            string metot = "Kullanici/oku";


            string url = string.Concat(baseUrl, metot);
            if (token == null)
            {
                return BadRequest("Token yok olm nereye gidiyorsun");
            }


            var content = new StringContent(JsonConvert.SerializeObject(sorgu), Encoding.UTF8, "application/json");


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
    }
}
