using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Study1CApi.Controllers;
using Study1CApi.DTOs.CourseDTOs;
using Study1CApi.Models;
using Study1CApi.Repositories;
using System.Security.Claims;

namespace Study1CApiTests
{
    public class CourseControllerTests
    {
        private readonly Study1cDbContext _context;
        private readonly Mock<UserManager<AuthUser>> _userManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly DefaultHttpContext _mockHttpContext;
        private readonly User _adminData;
        private readonly AuthUser _adminLogin;
        private readonly Course _course;

        public CourseControllerTests()
        {
            _userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);
            _roleManagerMock = new Mock<RoleManager<Role>>(
                new Mock<IRoleStore<Role>>().Object,
                null,
                null,
                null,
                null);

            Guid id = Guid.NewGuid();

            _adminLogin = new AuthUser()
            {
                Id = id,
                Email = "admin@admin.com"
            };

            _adminData = new User()
            {
                UserId = id,
                UserSurname = "admin",
                UserName = "admin",
                UserPatronymic = "",
                AuthUserNavigation = _adminLogin
            };

            _course = new Course()
            {
                CourseId = Guid.NewGuid(),
                CourseName = "Курс",
                CourseDataCreate = DateTime.Now,
                Author = id
            };

            _context = GetDatabaseContext().Result;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _adminLogin.Email),
                new Claim(ClaimTypes.NameIdentifier, _adminLogin.Id.ToString())
            };

            _mockHttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims))
            };
        }

        private async Task<Study1cDbContext> GetDatabaseContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<Study1cDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            var databaseContext = new Study1cDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (await databaseContext.Users.CountAsync() <= 0)
            {
                databaseContext.Users.Add(_adminData);
                databaseContext.UserRoles.Add(new IdentityUserRole<Guid>() { RoleId = new Guid("f45d2396-3e72-4ec7-b892-7bd454248158"), UserId = _adminLogin.Id });
                await databaseContext.SaveChangesAsync();
            }

            if (await databaseContext.Courses.CountAsync() <= 0)
            {
                databaseContext.Courses.Add(_course);
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async Task CourseControllerTests_GetAllCourses_ReturnOKAsync()
        {

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(_adminLogin.Email)).
                ReturnsAsync(new AuthUser { UserName = _adminLogin.Email, Email = _adminLogin.Email, Id = _adminLogin.Id });

            _userManagerMock
                .Setup(um => um.GetRolesAsync(It.Is<AuthUser>(u => u.Id == _adminLogin.Id)))
                .ReturnsAsync(new List<string> { "Администратор" });

            var userRepository = new UserRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
            var courseRepository = new CourseRepository(_context, _userManagerMock.Object);

            var controller = new CourseController(courseRepository, userRepository, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext
                }
            };

            var result = await controller.GetAllCourses();

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task CourseControllerTests_GetCourseById_ReturnOKAsync()
        {
            var userRepository = new UserRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
            var courseRepository = new CourseRepository(_context, _userManagerMock.Object);

            var controller = new CourseController(courseRepository, userRepository, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext
                }
            };

            var result = await controller.GetCourseById(_course.CourseId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task CourseControllerTests_AddCourse_ReturnOKAsync()
        {
            var dto = new AddCourseDTO()
            {
                Title = "Курс2",
                Author = _adminData.UserId
            };

            var userRepository = new UserRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
            var courseRepository = new CourseRepository(_context, _userManagerMock.Object);

            var controller = new CourseController(courseRepository, userRepository, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext
                }
            };

            var result = await controller.AddCourse(dto);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task CourseControllerTests_UpdateCourse_ReturnOKAsync()
        {
            var dto = new UpdateCourseDTO()
            {
                CourseId = _course.CourseId,
                Title = "Курс2"
            };

            _userManagerMock
               .Setup(um => um.FindByEmailAsync(It.Is<string>(email => email == _adminLogin.Email)))
               .ReturnsAsync(new AuthUser { UserName = _adminData.UserName, Email = _adminLogin.Email, Id = _adminData.UserId });

            _userManagerMock
                .Setup(um => um.GetRolesAsync(It.Is<AuthUser>(u => u.Id == _adminData.UserId)))
                .ReturnsAsync(new List<string> { "Администратор" });

            var userRepository = new UserRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
            var courseRepository = new CourseRepository(_context, _userManagerMock.Object);

            var controller = new CourseController(courseRepository, userRepository, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext
                }
            };

            var result = await controller.UpdateCourse(dto);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task CourseControllerTests_DeleteCourse_ReturnOKAsync()
        {
            _userManagerMock
               .Setup(um => um.FindByEmailAsync(It.Is<string>(email => email == _adminLogin.Email)))
               .ReturnsAsync(new AuthUser { UserName = _adminData.UserName, Email = _adminLogin.Email, Id = _adminData.UserId });

            _userManagerMock
                .Setup(um => um.GetRolesAsync(It.Is<AuthUser>(u => u.Id == _adminData.UserId)))
                .ReturnsAsync(new List<string> { "Администратор" });

            var userRepository = new UserRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
            var courseRepository = new CourseRepository(_context, _userManagerMock.Object);

            var controller = new CourseController(courseRepository, userRepository, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = _mockHttpContext
                }
            };

            var result = await controller.DeleteCourse(_course.CourseId);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }
    }
}
