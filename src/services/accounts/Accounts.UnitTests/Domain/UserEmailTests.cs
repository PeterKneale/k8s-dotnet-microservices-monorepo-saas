using System;
using Accounts.Domain;
using FluentAssertions;
using Xunit;

namespace Accounts.UnitTests.Domain
{
    public class UserEmailTests
    {
        [Fact]
        public void VerifyEmail_UnverifiedUser_EmailIsVerified()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var email = "user@example.com";
            var user = new User(tenantId, userId, email);
            string token = user.EmailVerificationToken!;

            // Act
            user.VerifyEmail(token);

            // Assert
            user.EmailVerified.Should().BeTrue();
        }
    }

}
