using Expenses.Core.DTO;
using Expenses.Core.Utilities;
using Expenses.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Expenses.Core
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if(dbUser == null || !dbUser.Password.Equals(Hash.Password(user.Password)))
            {
                throw new Exception("Invalid username or password");
            }

            return new AuthenticatedUser()
            {
                Username = user.Username,
                Token = JwtToken.GenerateAuthToken(user.Username),
            };
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUsername = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if (checkUsername != null)
            {
                throw new Exception("Username already exists");
            }

            user.Password = Hash.Password(user.Password);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtToken.GenerateAuthToken(user.Username)
            };
        }
    }
}
