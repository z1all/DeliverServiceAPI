using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ASPDotNetWebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponseDTO>> Register([FromBody] RegistrationRequestDTO model)
        {
            var isNotUnique = await _userRepository.EmailIsUsedAsync(model.Email);

            if (isNotUnique)
            {
                return BadRequest(new ResponseDTO()
                {
                    Status = 400,
                    Message = $"Username '{model.Email}' is already taken."
                });
            }

            var token = await _userRepository.RegisterAsync(model);

            return Ok(token);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponseDTO>> Login([FromBody] LoginRequestDTO model)
        {
            var token = await _userRepository.LoginAsync(model);

            if (token == null)
            {
                return BadRequest(new ResponseDTO()
                {
                    Status = 400,
                    Message = $"Login failed."
                });
            }

            return Ok(token);
        }

        [HttpPost("logout")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO>> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var IsLoggedOut = await _userRepository.LogoutAsync(token);
            if (!IsLoggedOut)
            {
                return BadRequest(new ResponseDTO()
                {
                    Status = 400,
                    Message = "Invalid JWT token."
                });
            }

            return new ResponseDTO()
            {
                Status = null,
                Message = "Logged out."
            };
        }

        [HttpGet("profile")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDTO>> GetUserInfo()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(token);
            var userGuidStr = parsedToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            if (userGuidStr == null)
            {
                HttpContext.Response.Headers.Add("JWTToken", "Error: a token without a UserId.");
                return Forbid();
            }

            if (!Guid.TryParse(userGuidStr.Value, out Guid userGuid))
            {
                HttpContext.Response.Headers.Add("JWTToken", "Error: a token contain an uncorrected userId");
                return Forbid();
            }

            var userInfo = await _userRepository.GetProfileAsync(userGuid);
            if (userInfo == null)
            {
                return NotFound(new ResponseDTO()
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User not found. Either it doesn't exist, or the token is invalid."
                });
            }

            return userInfo;
        }
    }
}
