using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ExpertClinicTestTask.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ExpertClinicTestTask.API.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //Я тут просто накидал хардкодом Login и Password без всякого шифрования, без вытягивания из базы
        //Самый простой захардкоденный Base Auth, тк в требованиях ничего не увидел за авторизацию, уж лучше, чем ничего
        
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                bool userModel = false;
                if(username.Equals("Artem") && password.Equals("Yarochkin"))
                {
                    userModel = true;
                }
                
                if (userModel)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail($"Invalid Authorization Header: {ex.Message}");
            }
        }
    }