using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;

            if (userManager != null) { _userManager = userManager; }
            if (signInManager != null) { _signinManager = signInManager; }
        }

        public List<UserDto> GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            return users;
        }

        public UserDto GetUser(string userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var User = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            return User;
        }

        public async Task<User> Register(UserDto userRegister)
        {
            if (userRegister == null)
                throw new BadRequestException();

            var user = _userRepository.GetUsers()
                .Where(a => a.Email.Trim().ToUpper() == userRegister.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
                throw new UnprocessableException("User already exists");

            var userMap = _mapper.Map<User>(userRegister);
            var createdUser = await _userManager.CreateAsync(userMap, userMap.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(userMap, "User");

                if (!roleResult.Succeeded)
                    throw new Exception();

                return userMap;
            }
            else
            {
                throw new Exception(string.Join(", ", createdUser.Errors.Select(x => "Code " + x.Code + " Description" + x.Description)));
            }
        }

        public async Task<User> Login(LoginDto userLogin)
        {
            if (userLogin == null)
                throw new BadRequestException();

            var user = _userRepository.GetUsers()
                .Where(u => u.UserName == userLogin.Username.ToLower())
                .FirstOrDefault();

            if (user == null)
                throw new UnauthorizedException("Invalid username");

            var result = await _signinManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

            if (!result.Succeeded)
                throw new UnauthorizedException("Username not found and/or password incorrect");

            return user;
        }

        public string CreateUser(UserDto userCreate)
        {
            if (userCreate == null)
                throw new BadRequestException();

            var user = _userRepository.GetUsers()
                .Where(a => a.Email.Trim().ToUpper() == userCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
                throw new UnprocessableException("User already exists");

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateUser(string userId, UserDto userUpdate)
        {
            if (userUpdate == null || userId != userUpdate.Id)
                throw new BadRequestException();

            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var userMap = _mapper.Map<User>(userUpdate);

            if (!_userRepository.UpdateUser(userMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteUser(string userId)
        {
            var user = _userRepository.GetUser(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            if (!_userRepository.DeleteUser(user))
                throw new Exception();

            return "Successfully deleted";
        }

        public List<ReviewDto> GetReviewsByUser(string userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var reviews = _mapper.Map<List<ReviewDto>>(_userRepository.GetReviewsByUser(userId));

            return reviews;
        }

        public List<OrderDto> GetOrdersByUser(string userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var reviews = _mapper.Map<List<OrderDto>>(_userRepository.GetOrdersByUser(userId));

            return reviews;
        }
    }
}
