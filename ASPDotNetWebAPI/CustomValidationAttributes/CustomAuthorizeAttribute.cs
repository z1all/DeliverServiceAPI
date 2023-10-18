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
        private readonly ApplicationDbContext _dbContext;

        public CustomAuthorizeAttribute(ApplicationDbContext context) 
        {
            _dbContext = context;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!tokenHandler.CanReadToken(token))
            {
                // Добавить ErrorResponseDTO
                context.Result = new UnauthorizedResult();
            }

            var parsedToken = tokenHandler.ReadJwtToken(token);
            var JTI = parsedToken.Claims.FirstOrDefault(claim => claim.Type == "JTI");

            if (JTI == null)
            {
                // Добавить ErrorResponseDTO
                context.Result = new UnauthorizedResult();
            }

            var deletedToken = await _dbContext.DeletedTokens.FirstOrDefaultAsync(token => token.TokenJTI == JTI.Value);
            
            if(deletedToken != null)
            {
                // Добавить ErrorResponseDTO
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
