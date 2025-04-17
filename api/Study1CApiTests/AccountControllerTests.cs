using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Study1CApi.Controllers;
using Study1CApi.DTOs.AccountDTOs;
using Study1CApi.DTOs.AuthDTO;
using Study1CApi.DTOs.UserDTOs;
using Study1CApi.Interfaces;
using Study1CApi.Models;
using Study1CApi.Repositories;

namespace Study1CApiTests
{
    public class AccountControllerTests
    {
        private readonly ITokenService _tokenService;
        private readonly Study1cDbContext _context;
        private Guid idProfile;

        public AccountControllerTests()
        {
            var config = new Mock<IConfiguration>();
            var configSectionMock = new Mock<IConfigurationSection>();
            config.Setup(conf => conf["JWT:SigningKey"]).Returns("s3cr3t!k3y[f0r=study.1C.jwt.auth.educ@t10n4kjeh37i4fhwifh9-wfj;2ojq3f09q2fjhoidfgh8723tfiwkfgs8w7fsf");
            configSectionMock.Setup(x => x.Value).Returns("40.0");
            config.Setup(conf => conf.GetSection("JWT:TokenValidMinutes"))
                .Returns(configSectionMock.Object);
            config.Setup(conf => conf["JWT:Issuer"]).Returns("https://localhost:7053");
            config.Setup(conf => conf["JWT:Audience"]).Returns("study.1C.client");
            _tokenService = new TokenService(config.Object);
            _context = GetDatabaseContext().Result;
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
                idProfile = Guid.NewGuid();
                databaseContext.Users.Add(
                    new User()
                    {
                        UserId = idProfile,
                        UserSurname = "admin",
                        UserName = "admin",
                        UserPatronymic = "",
                        AuthUserNavigation = new AuthUser()
                        {
                            Id = idProfile,
                            Email = "admin@admin.com",
                        }
                    });
                databaseContext.UserRoles.Add(new IdentityUserRole<Guid>() { RoleId = new Guid("f45d2396-3e72-4ec7-b892-7bd454248158"), UserId = idProfile });
                await databaseContext.SaveChangesAsync();

            }
            return databaseContext;
        }

        [Fact]
        public async Task AccountControllerTests_Login_ReturnOKAsyncAndToken()
        {
            var dto = new LoginDTO()
            {
                Email = "admin@admin.com",
                Password = "admin1cdbapi"
            };

            var userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);

            userManagerMock
                .Setup(userManager => userManager
                .CreateAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.FindByEmailAsync(dto.Email)).
                ReturnsAsync(new AuthUser { UserName = dto.Email, Email = dto.Email, Id = idProfile });

            userManagerMock
                .Setup(um => um.GetRolesAsync(It.Is<AuthUser>(u => u.Id == idProfile)))
                .ReturnsAsync(new List<string> { "Администратор" });

            var signInManager = new Mock<SignInManager<AuthUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AuthUser>>(), null, null, null, null);

            signInManager
                .Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AuthUser>(), dto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var roleStoreMock = new Mock<IRoleStore<Role>>();

            var mockRoleManager = new Mock<RoleManager<Role>>(
                roleStoreMock.Object,
                null,
                null,
                null,
                null
            );

            var accountRepository = new AccountRepository(_context);

            var controller = new AccountController(userManagerMock.Object, _tokenService, signInManager.Object, mockRoleManager.Object, _context, accountRepository);

            var result = await controller.Login(dto);

            if (result is OkObjectResult okResult)
            {
                var resultDto = (SignInDTO)okResult.Value;
                Assert.NotNull(resultDto.Token);
            }

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task AccountControllerTests_Register_ReturnOKAsync()
        {
            var dto = new RegistrationDTO()
            {
                UserSurname = "curator",
                UserName = "curator",
                UserPatronymic = "",
                Email = "curator@curator.com",
                Password = "12345678",
                ConfirmPassword = "12345678",
                RoleId = new Guid("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e")
            };

            var userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);

            userManagerMock
                .Setup(userManager => userManager
                .CreateAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock
                .Setup(um => um.FindByEmailAsync(dto.Email)).
                ReturnsAsync(new AuthUser { UserName = dto.Email, Email = dto.Email });
            userManagerMock
                .Setup(um => um.GetRolesAsync(It.Is<AuthUser>(u => u.Id == idProfile)))
                .ReturnsAsync(new List<string> { "Администратор" });

            var signInManager = new Mock<SignInManager<AuthUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AuthUser>>(), null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<Role>>();

            var mockRoleManager = new Mock<RoleManager<Role>>(
               roleStoreMock.Object,
               null,
               null,
               null,
               null
           );

            mockRoleManager.Setup(x => x.FindByIdAsync("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"))
                   .ReturnsAsync(new Role { Id = Guid.Parse("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"), Name = "Куратор" });

            var accountRepository = new AccountRepository(_context);

            var controller = new AccountController(userManagerMock.Object, _tokenService, signInManager.Object, mockRoleManager.Object, _context, accountRepository);

            var result = await controller.Register(dto);

            if (result is OkObjectResult okResult)
            {
                var resultDto = (UserDTO)okResult.Value;
                var newUserInContext = _context.Users.FirstOrDefault(it => it.UserId == resultDto.UserId);
                Assert.NotNull(newUserInContext);
            }

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task AccountControllerTests_Register_ReturnBadRequest()
        {
            var dto = new RegistrationDTO()
            {
                UserSurname = "curator",
                UserName = "curator",
                UserPatronymic = "",
                Email = "curator@curator.com",
                Password = "12345678",
                ConfirmPassword = "12345678",
                RoleId = new Guid("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e")
            };

            var userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);

            userManagerMock
                .Setup(userManager => userManager
                .CreateAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName" })));

            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.FindByEmailAsync(dto.Email)).
                ReturnsAsync(new AuthUser { UserName = dto.Email, Email = dto.Email });

            var signInManager = new Mock<SignInManager<AuthUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AuthUser>>(), null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<Role>>();

            var mockRoleManager = new Mock<RoleManager<Role>>(
               roleStoreMock.Object,
               null,
               null,
               null,
               null
           );

            mockRoleManager.Setup(x => x.FindByIdAsync("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"))
                  .ReturnsAsync(new Role { Id = Guid.Parse("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"), Name = "Куратор" });

            var accountRepository = new AccountRepository(_context);

            var controller = new AccountController(userManagerMock.Object, _tokenService, signInManager.Object, mockRoleManager.Object, _context, accountRepository);

            var result = await controller.Register(dto);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AccountControllerTests_Login_ReturnUnauthorized()
        {
            var dto = new LoginDTO()
            {
                Email = "admin@admin.com",
                Password = "admin1cdbapi"
            };

            var userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);

            userManagerMock
                .Setup(userManager => userManager
                .CreateAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.FindByEmailAsync(dto.Email)).
                ReturnsAsync(new AuthUser { UserName = dto.Email, Email = dto.Email, Id = idProfile });

            var signInManager = new Mock<SignInManager<AuthUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AuthUser>>(), null, null, null, null);

            signInManager
                .Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AuthUser>(), dto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var roleStoreMock = new Mock<IRoleStore<Role>>();

            var mockRoleManager = new Mock<RoleManager<Role>>(
                roleStoreMock.Object,
                null,
                null,
                null,
                null
            );

            var accountRepository = new AccountRepository(_context);

            var controller = new AccountController(userManagerMock.Object, _tokenService, signInManager.Object, mockRoleManager.Object, _context, accountRepository);

            var result = await controller.Login(dto);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task AccountControllerTests_UpdateProfile_ReturnOKAsync()
        {
            var dto = new UpdateProfileDTO()
            {
                UserId = idProfile,
                Email = "admin@admin.com",
                UserSurname = "admin1",
                UserName = "admin",
                UserPatronymic = "",
            };

            var auth = new LoginDTO()
            {
                Email = "admin@admin.com",
                Password = "admin1cdbapi"
            };

            var userManagerMock = new Mock<UserManager<AuthUser>>(
                new Mock<IUserStore<AuthUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AuthUser>>().Object,
                new IUserValidator<AuthUser>[0],
                new IPasswordValidator<AuthUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AuthUser>>>().Object);

            userManagerMock
                .Setup(userManager => userManager
                .CreateAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<AuthUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            userManagerMock
                .Setup(um => um.FindByEmailAsync(dto.Email)).
                ReturnsAsync(new AuthUser { UserName = dto.Email, Email = dto.Email, Id = idProfile });

            var signInManager = new Mock<SignInManager<AuthUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AuthUser>>(), null, null, null, null);

            signInManager
                .Setup(s => s.CheckPasswordSignInAsync(It.IsAny<AuthUser>(), auth.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var roleStoreMock = new Mock<IRoleStore<Role>>();

            var mockRoleManager = new Mock<RoleManager<Role>>(
                roleStoreMock.Object,
                null,
                null,
                null,
                null
            );

            var accountRepository = new AccountRepository(_context);

            var controller = new AccountController(userManagerMock.Object, _tokenService, signInManager.Object, mockRoleManager.Object, _context, accountRepository);

            var result = await controller.UpdateProfile(dto);

            if (result is OkObjectResult okResult)
            {
                var resultDto = (UserDTO)okResult.Value;
                var newUserInContext = _context.Users.FirstOrDefault(it => it.UserId == resultDto.UserId);
                Assert.NotNull(newUserInContext);
            }

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<UnauthorizedObjectResult>();
        }
    }
}
