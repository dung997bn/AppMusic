﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace Data.Extensions
{
    public static class IdentityExtension
    {
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }

        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            IdentityOptions options = new IdentityOptions();
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity).Claims.Single(x => x.Type == options.ClaimsIdentity.UserIdClaimType);
            return Guid.Parse(claim.Value);
        }
    }
}
