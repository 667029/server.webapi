﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Core.DTO;
using System.Linq;

namespace System.Core
{
    public class ExpensesServices : IExpensesServices
    {
        private readonly System.DB.AppDbContext _context;
        private readonly System.DB.User _user;

        public ExpensesServices(System.DB.AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                .First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Expense CreateExpense(System.DB.Expense expense)
        {
            expense.User = _user;
            _context.Add(expense);
            _context.SaveChanges();

            return (Expense)expense;
        }

        public void DeleteExpense(Expense expense)
        {
            var dbExpense = _context.Expenses.First(e => e.User.Id == _user.Id && e.Id == expense.Id);
            _context.Remove(dbExpense);
            _context.SaveChanges();
        }

        public Expense EditExpense(Expense expense)
        {
            var dbExpense = _context.Expenses
                 .Where(e => e.User.Id == _user.Id && e.Id == expense.Id)
                 .First();
            dbExpense.Description = expense.Description;
            dbExpense.Amount = expense.Amount;
            _context.SaveChanges();

            return expense;
        }

        public Expense GetExpense(int id) =>
            _context.Expenses
                .Where(e => e.User.Id == _user.Id && e.Id == id)
                .Select(e => (Expense)e)
                .First();

        public List<Expense> GetExpenses() =>
            _context.Expenses
                .Where(e => e.User.Id == _user.Id)
                .Select(e => (Expense)e)
                .ToList();
    }
}
