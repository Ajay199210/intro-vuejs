// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("web2Api", new string[] { JwtClaimTypes.Scope, JwtClaimTypes.Role })
                {
                    Scopes = { "web2ApiScope" }
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("web2ApiScope")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // Interactive client using code flow + pkce
                new Client
                {
                    ClientId = "swagger_ui",
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:7152/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins = { "https://localhost:7152" },
                    RequireClientSecret = false,
                    RequirePkce = false,

                    AllowedScopes = { "web2ApiScope" }
                },

                new Client
                {
                    ClientId = "web2_ui",
                    ClientName = "Web2.UI Vuejs oidc client",
                    ClientSecrets = {new Secret("secret".Sha256())}, // change me!
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = {
                    "http://localhost:8080/auth/signinsilent/vuejs",
                    "http://localhost:8080/auth/signinwin/vuejs",
                    "http://localhost:8080/auth/signinpop/vuejs"
                    },
                    PostLogoutRedirectUris = {"http://localhost:8080/" },
                    AllowedCorsOrigins = {"http://localhost:8080"},
                    AllowedScopes = { "web2ApiScope", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,}
                }
            };
    }
}