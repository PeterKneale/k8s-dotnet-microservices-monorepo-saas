using System;
using Accounts.Domain;
using FluentAssertions;
using Xunit;

namespace Accounts.UnitTests.Domain
{
    public class UserPasswordTests
    {
        private readonly User _user;
        
        public UserPasswordTests()
        {
            var tenantId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var email = "user@example.com";
            _user = new User(tenantId, userId, email);
        }
        
        [Fact]
        public void SetPassword_VerifiedEmail_PasswordIsSet()
        {
            // Arrange
            _user.VerifyEmail(_user.EmailVerificationToken!);

            // Act
            _user.SetPassword("secret");

            // Assert
            _user.Password.Should().Be("secret");
        }
        
        [Fact]
        public void SetPassword_UnVerifiedEmail_Throws()
        {
            // Arrange

            // Act
            Action act = ()=> _user.SetPassword("secret");

            // Assert
            act.Should().Throw<Exception>().WithMessage("*not verified*");
        }
    }
}