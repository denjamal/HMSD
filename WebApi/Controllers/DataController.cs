using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ICipher _cipher;

        public DataController(ICipher cipher)
        {
            _cipher = cipher;
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] string payload)
        {
            var result = _cipher.Encrypt(payload);
            
            return Ok(result);
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] string payload)
        {
            var result = _cipher.Decrypt(payload);

            return Ok(result);
        }
    }
}
