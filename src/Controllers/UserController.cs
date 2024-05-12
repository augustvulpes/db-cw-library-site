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

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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
                return NotFound(new { message = e.Message });
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
                return NotFound(new { message = e.Message });
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
                return NotFound(new { message = e.Message });
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
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something went wrong while regist");
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
                return BadRequest();
            }
            catch (UnauthorizedException e)
            {
                return StatusCode(401, new { message = e.Message });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Something went wrong while regist");
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
                return BadRequest();
            }
            catch (UnprocessableException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while creating");
                return StatusCode(500, ModelState);
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
                return BadRequest();
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
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
                return NotFound(new { message = e.Message });
            }
            catch (Exception _)
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
        }
    }
}
