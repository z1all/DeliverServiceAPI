using ASPDotNetWebAPI.Helpers;
using ASPDotNetWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await CheckTokenJTI(context);
            CheckTokenUserId(context);
        }

        private async Task CheckTokenJTI(AuthorizationFilterContext context)
        {
            var JTI = JWTTokenHelper.GetJTIFromToken(context.HttpContext);
            var userId = JWTTokenHelper.GetUserIdFromToken(context.HttpContext);

            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            var accessToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.AccessTokenJTI == JTI && refreshToken.UserId == userId);
            if (accessToken == null)
            {
                Forbid(context, "JWTToken", "Error: this token is no longer available!");
                return;
            }
        }

        private void CheckTokenUserId(AuthorizationFilterContext context)
        {
            var userGuidStr = JWTTokenHelper.GetValueFromToken(context.HttpContext, "UserId");

            if (userGuidStr == null)
            {
                Forbid(context, "JWTToken", "Error: a token without a UserId.");
                return;
            }

            if (!Guid.TryParse(userGuidStr, out Guid userGuid))
            {
                Forbid(context, "JWTToken", "Error: a token contain an uncorrected userId");
                return;
            }
        }

        private void Forbid(AuthorizationFilterContext context, string key, string message)
        {
            context.HttpContext.Response.Headers.Add(key, message);
            context.Result = new ForbidResult();
        }
    }
}
