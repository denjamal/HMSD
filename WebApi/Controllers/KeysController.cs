using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly IKeyService _keyService;

        public KeysController(IKeyService keyService)
        {
            _keyService = keyService;
        }

        [HttpPost("rotate")]
        public IActionResult Rotate()
        {
            _keyService.RotateEncryptionKey();

            return NoContent();
        }

    }
}
