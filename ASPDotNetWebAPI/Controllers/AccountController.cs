using ASPDotNetWebAPI.CustomValidationAttributes;
using ASPDotNetWebAPI.Helpers;
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

        /// <summary>
        /// Register new user
        /// </summary>
        /// <remarks>
        /// The method can return BadReqeust if the validation of the model data is unsuccessful:
        /// 
        ///     {
        ///         "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ///         "title": "One or more validation errors occurred.",
        ///         "status": 400,
        ///         "traceId": "00-84074cbf8bec5d381fa4d51ceeab960a-b35b969b8d3e8088-00",
        ///         "errors": {
        ///             "Password": [
        ///                 "The password must contain at least one digit and one capital letter."
        ///             ],
        ///             "PhoneNumber": [
        ///                 "The phone number must start with '+7' or '8' and have 11 digits."
        ///             ]
        ///         }
        ///     }
        /// </remarks>
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

            return token;
        }

        /// <summary>
        /// Log in to the system
        /// </summary>
        /// <remarks>
        /// The method can return BadReqeust if the validation of the model data is unsuccessful:
        /// 
        ///     {
        ///         "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ///         "title": "One or more validation errors occurred.",
        ///         "status": 400,
        ///         "traceId": "00-84074cbf8bec5d381fa4d51ceeab960a-b35b969b8d3e8088-00",
        ///         "errors": {
        ///             "Password": [
        ///                 "The password must contain at least one digit and one capital letter."
        ///             ],
        ///             "PhoneNumber": [
        ///                 "The phone number must start with '+7' or '8' and have 11 digits."
        ///             ]
        ///         }
        ///     }
        /// </remarks>
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

        /// <summary>
        /// Log out system user
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("logout")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO>> Logout()
        {
            var token = JWTTokenHelper.GetTokenFromHeader(HttpContext);

            await _userRepository.LogoutAsync(token);

            return new ResponseDTO()
            {
                Status = null,
                Message = "Logged out."
            };
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("profile")]
        [CustomAuthorize]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDTO>> GetUserInfo()
        {
            var token = JWTTokenHelper.GetTokenFromHeader(HttpContext);

            var userInfo = await _userRepository.GetProfileAsync(token);
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

        /// <summary>
        /// Edit user profile
        /// </summary>
        /// <remarks>
        /// The method can return BadReqeust if the validation of the model data is unsuccessful:
        /// 
        ///     {
        ///         "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        ///         "title": "One or more validation errors occurred.",
        ///         "status": 400,
        ///         "traceId": "00-84074cbf8bec5d381fa4d51ceeab960a-b35b969b8d3e8088-00",
        ///         "errors": {
        ///             "Password": [
        ///                 "The password must contain at least one digit and one capital letter."
        ///             ],
        ///             "PhoneNumber": [
        ///                 "The phone number must start with '+7' or '8' and have 11 digits."
        ///             ]
        ///         }
        ///     }
        /// </remarks>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPut("profile")]
        [CustomAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EditUserInfo([FromBody] UserEditRequestDTO model)
        {
            var token = JWTTokenHelper.GetTokenFromHeader(HttpContext);

            var hasBeenUpdated = await _userRepository.EditProfileAsync(token, model);
            if (!hasBeenUpdated)
            {
                return NotFound(new ResponseDTO()
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User not found. Either it doesn't exist, or the token is invalid."
                });
            }

            return Ok();
        }
    }
}
