using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserDto> GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

            return users;
        }

        public UserDto GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var User = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            return User;
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

        public string UpdateUser(int userId, UserDto userUpdate)
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

        public string DeleteUser(int userId)
        {
            var user = _userRepository.GetUser(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            if (!_userRepository.DeleteUser(user))
                throw new Exception();

            return "Successfully deleted";
        }

        public List<ReviewDto> GetReviewsByUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var reviews = _mapper.Map<List<ReviewDto>>(_userRepository.GetReviewsByUser(userId));

            return reviews;
        }

        public List<OrderDto> GetOrdersByUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                throw new NotFoundException("User not found");

            var reviews = _mapper.Map<List<OrderDto>>(_userRepository.GetOrdersByUser(userId));

            return reviews;
        }
    }
}
