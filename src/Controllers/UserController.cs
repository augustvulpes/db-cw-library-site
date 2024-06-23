using LibraryApp.Dto;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(string userId)
        {
            try
            {
                var user = _userService.GetUser(userId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(user);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("reviews/{userId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByUser(string userId)
        {
            try
            {
                var reviews = _userService.GetReviewsByUser(userId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(reviews);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpGet("orders/{userId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrdersByUser(string userId)
        {
            try
            {
                var orders = _userService.GetOrdersByUser(userId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(orders);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] UserDto userRegister)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userService.Register(userRegister);

                return Ok(new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while register");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginDto userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userService.Login(userLogin);

                return Ok(new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnauthorizedException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(401, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while login");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            try
            {
                var response = _userService.CreateUser(userCreate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(string userId, [FromBody] UserDto userUpdate)
        {
            try
            {
                var response = _userService.UpdateUser(userId, userUpdate);

                return Ok(response);
            }
            catch (BadRequestException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                var response = _userService.DeleteUser(userId);

                return Ok(response);
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                ModelState.AddModelError("", "Something went wrong while regist");
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
