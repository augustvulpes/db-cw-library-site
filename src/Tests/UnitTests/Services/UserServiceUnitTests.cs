using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Interfaces.RepositoryInterfaces;
using Xunit;
using Moq;
using LibraryApp.Services;
using AutoMapper;
using LibraryApp.Helper;

namespace LibraryApp.Tests.UnitTests.Services
{
    public class UserServiceUnitTests
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public readonly Mock<IUserRepository> _userRepositoryMock = new();

        public UserServiceUnitTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            var mapper = config.CreateMapper();

            _userService = new UserService(_userRepositoryMock.Object, mapper, null, null);

            _mapper = mapper;
        }
    }
}
