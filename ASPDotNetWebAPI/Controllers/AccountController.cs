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
        [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<TokenResponseDTO> Register([FromBody] RegistrationRequestDTO model)
        {
            return await _userRepository.RegisterAsync(model);
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
        [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<TokenResponseDTO> Login([FromBody] LoginRequestDTO model)
        {
            return await _userRepository.LoginAsync(model);
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
        public async Task<ResponseDTO> Logout()
        {
            var JTI = JWTTokenHelper.GetJTIFromToken(HttpContext);
            await _userRepository.LogoutAsync(JTI);

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
        public async Task<UserResponseDTO> GetUserInfo()
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);

            return await _userRepository.GetProfileAsync(userId);
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
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPut("profile")]
        [CustomAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task EditUserInfo([FromBody] UserEditRequestDTO model)
        {
            var userId = JWTTokenHelper.GetUserIdFromToken(HttpContext);
            await _userRepository.EditProfileAsync(userId, model);
        }
    }
}
