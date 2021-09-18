using System;

namespace System.Core.DTO
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public static explicit operator Expense(System.DB.Expense e) => new Expense
        {
            Id = e.Id,
            Description = e.Description,
            Amount = e.Amount
        };
    }
}
