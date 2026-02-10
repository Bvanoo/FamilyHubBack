using FamHubBack.Data;
using FamHubBack.Data.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamHubBack.Attributes;

namespace FamHubBack.Controllers
{
    public class LoginController : Controller
    {
        public readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        [GoogleAuthorize]
        [HttpPost("api/auth/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] TokenRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
            var email = payload.Email;

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == payload.Name);
            if (user == null)
            {
                user = new User { Email = email, Name = payload.Name, PasswordHash = payload.JwtId };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            var emailTest = HttpContext.Items["GoogleUserEmail"]?.ToString();

            //A utilser pour remplacer le token google par le mien 
            //var serverToken = GenerateServerJwt(user);

            return Ok(new
            {
                emailTest = emailTest,
                //token = serverToken,
                redirectUrl = "/home"
            });
        }

    }
    public class TokenRequest
    {
        public string Token { get; set; } = null!;
    }
}
