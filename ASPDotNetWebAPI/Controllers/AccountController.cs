using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

                // Вариант из API 
                //var errors = new Dictionary<string, string[]>
                //{
                //    { "DuplicateUserName", new[] { $"Username '{model.Email}' is already taken." } }
                //};
                //return BadRequest(errors);
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
            if(token == null)
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
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO>> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            var IsLoggedOut = await _userRepository.LogoutAsync(token);
            if(!IsLoggedOut)
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
    }
}
