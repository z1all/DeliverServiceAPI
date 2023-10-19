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

            var JTI = parsedToken.Claims.FirstOrDefault(claim => claim.Type == "JTI");

            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            var deletedToken = await dbContext.DeletedTokens.FirstOrDefaultAsync(token => token.TokenJTI == JTI.Value);
            
            if(deletedToken != null)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
