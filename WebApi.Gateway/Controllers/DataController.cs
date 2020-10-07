using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Gateway.Settings;

namespace WebApi.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly EncryptionServiceApi _encryptionServiceApi;

        public DataController(IHttpClientFactory factory, EncryptionServiceApi encryptionServiceApi)
        {
            _encryptionServiceApi = encryptionServiceApi;
            var client = factory.CreateClient();
            client.BaseAddress = new Uri(_encryptionServiceApi.BaseUrl);
            _httpClient = client;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptAsync([FromBody] string payload)
        {
            var requestData = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(_encryptionServiceApi.EncryptUrl, new StringContent(requestData, Encoding.UTF8, "application/json"));

            try
            {
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> DecryptAsync([FromBody] string payload)
        {
            var requestData = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(_encryptionServiceApi.DecryptUrl, new StringContent(requestData, Encoding.UTF8, "application/json"));

            try
            {
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
        }

        [HttpPost("rotateKeys")]
        public async Task<IActionResult> RotateAsync()
        {
            var response = await _httpClient.PostAsync(_encryptionServiceApi.RotateUrl, null);

            try
            {
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
        }
    }
}
