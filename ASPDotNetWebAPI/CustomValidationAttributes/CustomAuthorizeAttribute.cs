using ASPDotNetWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ASPDotNetWebAPI.CustomValidationAttributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);

            await CheckTokenJTI(context, parsedToken);
            CheckTokenUserId(context, parsedToken);
        }

        private async Task CheckTokenJTI(AuthorizationFilterContext context, JwtSecurityToken token)
        {
            var JTI = token.Claims.FirstOrDefault(claim => claim.Type == "JTI");
            if (JTI == null)
            {   
                Forbid(context, "JWTToken", "Error: a token without a unique identifier JTI.");
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            var deletedToken = await dbContext.DeletedTokens.FirstOrDefaultAsync(token => token.TokenJTI == JTI.Value);
            if (deletedToken != null)
            {
                Forbid(context, "JWTToken", "Error: this token is no longer available");
                return;
            }
        }

        private void CheckTokenUserId(AuthorizationFilterContext context, JwtSecurityToken token)
        {
            var userGuidStr = token.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            if (userGuidStr == null)
            {
                Forbid(context, "JWTToken", "Error: a token without a UserId.");
                return;
            }

            if (!Guid.TryParse(userGuidStr.Value, out Guid userGuid))
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
