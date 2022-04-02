using System;
using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace Accounts.Domain
{
    public class User
    {
        public User(string accountId, string userId, string email)
        {
            Guard.Against.NullOrWhiteSpace(userId, nameof(userId));
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            AccountId = accountId;
            UserId = userId;
            Email = email;
            EmailVerified = false;
            EmailVerificationToken = Guid.NewGuid().ToString();
        }

        [ExcludeFromCodeCoverage]
        private User()
        {
        }

        public string AccountId { get;private set; }
        
        public string UserId { get; private set; }
        
        public string Email { get; private set; }
        
        public string? Password { get; private set; }
        
        public bool EmailVerified { get; private set; }
        
        public string? EmailVerificationToken { get; private set; }

        public void VerifyEmail(string token)
        {
            Guard.Against.NullOrWhiteSpace(token, nameof(token));
            if (EmailVerified)
            {
                throw new Exception("Email already verified");
            }
            if (EmailVerificationToken != token)
            {
                throw new Exception("Email verification failed");
            }
            EmailVerified = true;
            EmailVerificationToken = null;
        }
        
        public void SetPassword(string password)
        {
            Guard.Against.NullOrWhiteSpace(password, nameof(password));
            if (!EmailVerified)
            {
                throw new Exception("Email not verified");
            }
            Password = password;
        }
    }
}