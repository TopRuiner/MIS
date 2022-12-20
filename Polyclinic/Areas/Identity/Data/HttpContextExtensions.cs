﻿using Microsoft.AspNetCore.Identity;

namespace Polyclinic.Areas.Identity.Data
{
    public static class HttpContextExtensions
    {
        public static async Task RefreshLoginAsync(this HttpContext context)
        {
            if (context.User == null)
                return;

            // The example uses base class, IdentityUser, yours may be called 
            // ApplicationUser if you have added any extra fields to the model
            var userManager = context.RequestServices
                .GetRequiredService<UserManager<PolyclinicUser>>();
            var signInManager = context.RequestServices
                .GetRequiredService<SignInManager<PolyclinicUser>>();

            PolyclinicUser user = await userManager.GetUserAsync(context.User);

            if (signInManager.IsSignedIn(context.User))
            {
                await signInManager.RefreshSignInAsync(user);
            }
        }
    }
}
