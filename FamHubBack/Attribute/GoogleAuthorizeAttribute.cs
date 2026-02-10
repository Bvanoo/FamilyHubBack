using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FamHubBack.Attributes
{
    /// <summary>
    /// Custom Google Authentication authorize attribute which validates the bearer token.
    /// </summary>
    public class GoogleAuthorizeAttribute : TypeFilterAttribute
    {
        public GoogleAuthorizeAttribute() : base(typeof(GoogleAuthorizeFilter)) { }
    }


    public class GoogleAuthorizeFilter : IAuthorizationFilter
    {

        public GoogleAuthorizeFilter()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                // Verify Authorization header exists
                var headers = context.HttpContext.Request.Headers;
                if (!headers.ContainsKey("Authorization"))
                {
                    context.Result = new UnauthorizedResult();
                }
                var authHeader = headers["Authorization"].ToString();

                // Verify authorization header starts with bearer and has a token
                if (!authHeader.StartsWith("Bearer ") && authHeader.Length > 7)
                {
                    context.Result = new UnauthorizedResult();
                }

                // Grab the token and verify through google. If verification fails, and exception will be thrown.
                var token = authHeader.Remove(0, 7);
                var validated = GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings()
                {
                    HostedDomain = "yourdomain.com",
                }).Result;
                context.HttpContext.Items["GoogleUserEmail"] = validated.Email;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}