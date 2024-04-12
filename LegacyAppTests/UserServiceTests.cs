using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_FirstName_Missing()
    {
        //Arrange
        var userService = new UserService();
            
        //Act
        var result = userService.AddUser(
            "",
            "Malewski",
            "malewski@gmail.pl",
            DateTime.Parse("2015-10-10"),
            10
        );
        
        //Assert
        Assert.False(result);
    }
}