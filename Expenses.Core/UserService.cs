using Expenses.Core.CustomExceptions;
using Expenses.Core.DTO;
using Expenses.Core.Utilities;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Expenses.Core
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if(dbUser == null || dbUser.Password == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }

            return new AuthenticatedUser()
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateAuthToken(user.Username),
            };
        }

        public async Task<AuthenticatedUser> ThirdPartySignIn(string token)
        {
            var payload = await ValidateAsync(token, new ValidationSettings
            {
                Audience = new[]
                {
                    Environment.GetEnvironmentVariable("CLIENT_ID")
                }
            });

            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(payload.Email));

            return dbUser == null
                ? await SignUp(new User
                    {
                        Username = GetUniqueUsernameFromEmail(payload.Email),
                        Email = payload.Email
                    })
                : new AuthenticatedUser()
                    {
                        Username = dbUser.Username,
                        Token = JwtGenerator.GenerateAuthToken(dbUser.Username),
                    };
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUsername = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if (checkUsername != null)
            {
                throw new UsernameAlreadyExistsException("Username already exists");
            }

            var checkEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(user.Email));

            if (checkEmail != null)
            {
                throw new EmailAlreadyRegisteredException("Email is already registered");
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
            }

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateAuthToken(user.Username)
            };
        }

        private string GetUniqueUsernameFromEmail(string email)
        {
            var emailSplit = email.Split('@').First();
            var random = new Random();
            var username = emailSplit;

            while (_context.Users.Any(u => u.Username.Equals(username)))
            {
                username = emailSplit + random.Next(10000000);
            }

            return username;
        }
    }
}
