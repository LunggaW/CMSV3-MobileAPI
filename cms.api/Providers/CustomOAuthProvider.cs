using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using cms.api.Models;
using System.Data.Entity;

namespace cms.api.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private CMSContext db = new CMSContext();
        
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            bool isValidUser = false;

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            KDSCMSUSER UserLogged = db.KDSCMSUSER.FirstOrDefault(d => 
                d.USERUSID == context.UserName && 
                d.USERPASW == context.Password && 
                d.USERSTAT == 1 && 
                d.USERTYPE == 1 &&
                DbFunctions.TruncateTime(d.USERSDAT) <= DbFunctions.TruncateTime(DateTime.Today) && 
                DbFunctions.TruncateTime(d.USEREDAT) >= DbFunctions.TruncateTime(DateTime.Today)
            );

            if (UserLogged != null)
            {
                isValidUser = true;
            }

            if (!isValidUser)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", UserLogged.USERSTPROF));
            context.Validated(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}