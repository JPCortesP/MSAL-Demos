using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MSAL_Auth_Utils
{
    /// <summary>
    /// Maybe unnecesary, but simple class that should be resusable among different platforms. 
    /// </summary>
    public class jpIdentityClient
    {
       
        private string RedirectUrl { get; } = "https://login.microsoftonline.com/common/oauth2/nativeclient";
        
        public IPublicClientApplication app;
        public string[] ScopesReadOnly = { "user.read", "User.ReadBasic.All", "Device.Read.All" };
        public jpIdentityClient(Secrets secrets, bool isWindows = true)
        {
            app = PublicClientApplicationBuilder.Create(secrets.ClientID)
                .WithRedirectUri(RedirectUrl)
                .WithTenantId(secrets.TenantID)
            .Build();
            if(isWindows)
                TokenCacheHelper.EnableSerialization(app.UserTokenCache); //this will ONLY works on Windows. Implement others
        }
    }
    /// <summary>
    /// Taken from https://learn.microsoft.com/en-us/entra/msal/dotnet/how-to/custom-token-cache-in-public-client-applications
    /// ONLY WORKS ON WINDOWS. 
    /// </summary>
    static class TokenCacheHelper
    {
        public static void EnableSerialization(ITokenCache tokenCache)
        {
            tokenCache.SetBeforeAccess(BeforeAccessNotification);
            tokenCache.SetAfterAccess(AfterAccessNotification);
        }

        /// <summary>
        /// Path to the token cache
        /// </summary>
        public static readonly string CacheFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location + ".msalcache.bin3";

        private static readonly object FileLock = new object();


        private static void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            lock (FileLock)
            {
                args.TokenCache.DeserializeMsalV3(File.Exists(CacheFilePath)
                        ? ProtectedData.Unprotect(File.ReadAllBytes(CacheFilePath),
                                                  null,
                                                  DataProtectionScope.CurrentUser)
                        : null);
            }
        }

        private static void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (args.HasStateChanged)
            {
                lock (FileLock)
                {
                    // reflect changes in the persistent store
                    File.WriteAllBytes(CacheFilePath,
                                        ProtectedData.Protect(args.TokenCache.SerializeMsalV3(),
                                                                null,
                                                                DataProtectionScope.CurrentUser)
                                        );
                }
            }
        }
    }
}
