using cms.api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NLog;

namespace cms.api.App_Start
{
    public partial class Startup
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void ConfigureOAuth(IAppBuilder app)
        {
            try
            {
                OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(5),
                    Provider = new CustomOAuthProvider()
                };

                // Token Generation
                app.UseOAuthAuthorizationServer(OAuthServerOptions);

                app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            }
            catch (Exception ex)
            {
                logger.Error("Error Message" + ex.Message);
                logger.Error("Inner Exception" + ex.InnerException);
                throw;
            }
          
        }
    }
}