using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace System.Core
{
    public class StatisticsServices : IStatisticsServices
    {
        private readonly System.DB.AppDbContext _context;
        private readonly System.DB.User _user;

        public StatisticsServices(System.DB.AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                .First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public IEnumerable<KeyValuePair<string, double>> GetExpenseAmountPerCategory() =>
            _context.Expenses
                .Where(e => e.User.Id == _user.Id)
                .AsEnumerable()
                .GroupBy(e => e.Description)
                .ToDictionary(e => e.Key, e => e.Sum(x => x.Amount))
                .Select(x => new KeyValuePair<string, double>(x.Key, x.Value));
    }
}
