using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(ITC.Startup))]
namespace ITC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                ClientId = "js",
                Authority = "https://secure.meyer-mil.com",
                //RedirectUri = "https://localhost:44398",
                RedirectUri = "http://milws/ITC/",
                ResponseType = "id_token token",
                RequireHttpsMetadata = false,
                MetadataAddress = @"https://secure.meyer-mil.com/.well-known/openid-configuration",
                Scope = "openid profile api1 email api",
                UseTokenLifetime = false,
                SignInAsAuthenticationType = "Cookies",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async n =>
                    {
                        var claims_to_exclude = new[]
                        {
                            "aud", "iss", "nbf", "exp", "nonce", "iat", "at_hash"
                        };

                        var claims_to_keep =
                            n.AuthenticationTicket.Identity.Claims
                            .Where(x => false == claims_to_exclude.Contains(x.Type)).ToList();
                        claims_to_keep.Add(new Claim("id_token", n.ProtocolMessage.IdToken));

                        if (n.ProtocolMessage.AccessToken != null)
                        {
                            claims_to_keep.Add(new Claim("access_token", n.ProtocolMessage.AccessToken));
                            var userInfoClient = new UserInfoClient(new Uri("https://secure.meyer-mil.com/connect/userinfo"), n.ProtocolMessage.AccessToken);
                            var userInfoResponse = await userInfoClient.GetAsync();
                            var userInfoClaims = userInfoResponse.Claims.Select(x => new Claim(x.Item1, x.Item2));
                            claims_to_keep.AddRange(userInfoClaims);
                        }

                        var ci = new ClaimsIdentity(
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            "name", "role");
                        ci.AddClaims(claims_to_keep);

                        n.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(
                            ci, n.AuthenticationTicket.Properties
                        );
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var id_token = n.OwinContext.Authentication.User.FindFirst("id_token")?.Value;
                            n.ProtocolMessage.IdTokenHint = id_token;
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
