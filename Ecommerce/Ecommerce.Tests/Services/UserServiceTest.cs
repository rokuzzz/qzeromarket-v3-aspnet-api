using Moq;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Services.UserService;
using Ecommerce.Services.UserService.DTO;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Common.Exceptions;
using Ecommerce.Domain.Filters;
using Ecommerce.Services.Common;
using Ecommerce.Services.UserService.Interfaces;

namespace Ecommerce.Tests.Services
{
  public class UserServiceTest
  {
    private readonly Mock<IUserRepo> _mockUserRepo;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<ISaltRepo> _mockSaltRepo;
    private readonly UserService _userService;

    public UserServiceTest()
    {
      _mockUserRepo = new Mock<IUserRepo>();
      _mockPasswordHasher = new Mock<IPasswordHasher>();
      _mockSaltRepo = new Mock<ISaltRepo>();
      _userService = new UserService(_mockUserRepo.Object, _mockPasswordHasher.Object, _mockSaltRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCalledWithExistingId_ReturnsGetDtoAsync()
    {
      // Arrange
      var userId = 1;
      var user = new User
      {
        Id = userId,
        Email = "test@example.com",
        FirstName = "John",
        LastName = "Doe",
        Password = "hashedpassword",
        Role = Role.User,
        Avatar = "avatar.jpg",

        Reviews = new List<Review>(),
        CartItems = new List<CartItem>(),
        Orders = new List<Order>(),
        SaltUser = new SaltUser { User = null!, UserId = userId, Salt = new byte[16] }
      };

      _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

      // Act
      var result = await _userService.GetByIdAsync(userId);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<GetUserDto>(result);
      Assert.Equal(user.Email, result.Email);
      Assert.Equal(user.FirstName, result.FirstName);
      Assert.Equal(user.LastName, result.LastName);
      Assert.Equal(user.Role.ToString(), result.Role);

      _mockUserRepo.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ReturnsPaginatedResult()
    {
      // Arrange
      var user = new User
      {
        Id = 1,
        Email = "test@example.com",
        FirstName = "John",
        LastName = "Doe",
        Password = "hashedpassword",
        Role = Role.User,
        Avatar = "avatar.jpg",

        Reviews = new List<Review>(),
        CartItems = new List<CartItem>(),
        Orders = new List<Order>(),
        SaltUser = new SaltUser { User = null!, UserId = 1, Salt = new byte[16] }
      };

      _mockUserRepo.Setup(repo => repo.GetAllAsync(It.IsAny<UserFilterOptions>())).ReturnsAsync([user]);
      _mockUserRepo.Setup(repo => repo.CountAsync(It.IsAny<UserFilterOptions>())).ReturnsAsync(1);

      var filteringOptions = new UserFilterOptions
      {
        Role = Role.User
      };

      // Act
      var result = await _userService.GetAllAsync(filteringOptions);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<PaginatedResult<User, GetUserDto>>(result);
      Assert.Equal(10, result.ItemsPerPage);
      Assert.Equal(1, result.CurrentPage);
      Assert.Equal(1, result.TotalItems);
      Assert.Single(result.Items);
      Assert.Equal("test@example.com", result.Items.First().Email);
      Assert.Equal("John", result.Items.First().FirstName);
      Assert.Equal("Doe", result.Items.First().LastName);

      _mockUserRepo.Verify(repo => repo.GetAllAsync(filteringOptions), Times.Once);
      _mockUserRepo.Verify(repo => repo.CountAsync(filteringOptions), Times.Once);
    }

    [Fact]
    public async Task PartialUpdateAsync_WhenCalledWithPartialUpdateDto_ReturnsGetDto()
    {
      // Arrange
      var userId = 1;
      var partialUpdateDto = new PartialUpdateUserDto
      {
        FirstName = "UpdatedJohn",
        LastName = "UpdatedDoe"
      };

      var existingUser = new User
      {
        Id = userId,
        Email = "test@example.com",
        FirstName = "John",
        LastName = "Doe",
        Password = "hashedpassword",
        Role = Role.User,
        Avatar = "avatar.jpg",

        Reviews = new List<Review>(),
        CartItems = new List<CartItem>(),
        Orders = new List<Order>(),
        SaltUser = new SaltUser { User = null!, UserId = userId, Salt = new byte[16] }
      };

      var updatedUser = new User
      {
        Id = userId,
        Email = existingUser.Email,
        FirstName = partialUpdateDto.FirstName,
        LastName = partialUpdateDto.LastName,
        Password = existingUser.Password,
        Role = existingUser.Role,
        Avatar = existingUser.Avatar,

        Reviews = existingUser.Reviews,
        CartItems = existingUser.CartItems,
        Orders = existingUser.Orders,
        SaltUser = existingUser.SaltUser
      };

      _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
      _mockUserRepo.Setup(repo => repo.PartialUpdateByIdAsync(It.IsAny<User>(), userId)).ReturnsAsync(updatedUser);

      // Act
      var result = await _userService.PartialUpdateByIdAsync(partialUpdateDto, userId);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<GetUserDto>(result);
      Assert.Equal(partialUpdateDto.FirstName, result.FirstName);
      Assert.Equal(partialUpdateDto.LastName, result.LastName);
      Assert.Equal(existingUser.Email, result.Email);

      _mockUserRepo.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
      _mockUserRepo.Verify(repo => repo.PartialUpdateByIdAsync(It.IsAny<User>(), userId), Times.Once);
    }

    [Fact]
    public async Task PartialUpdateByIdAsync_WhenCalledWithPartialUpdateDtoAndUserNotFound_ThrowsException()
    {
      // Arrange
      _mockUserRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new UserNotFoundException());

      var dto = new PartialUpdateUserDto
      {
        FirstName = "UpdatedJohn"
      };

      // Act & Assert
      await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.PartialUpdateByIdAsync(dto, 1));

      _mockUserRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
      _mockUserRepo.Verify(repo => repo.PartialUpdateByIdAsync(It.IsAny<User>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteByIdAsync_WhenCalledWithExistingId_ReturnsTrue()
    {
      // Arrange
      _mockUserRepo.Setup(repo => repo.DeleteByIdAsync(It.IsAny<int>())).ReturnsAsync(true);

      // Act
      var result = await _userService.DeleteByIdAsync(1);

      // Assert
      Assert.True(result);
      _mockUserRepo.Verify(repo => repo.DeleteByIdAsync(1), Times.Once);
    }
  }
}