using Budgeting_Application.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgeting_Application.Services
{
    public static class CalculatorService
    {
        private const int StartingAmount = 30000;

        public static object Integer { get; private set; }

        public static List<TransationDTO> CalculateTransactions(List<ExpectedDTO> expectedRows)
        {
            var transactions = new List<TransationDTO>();
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var finalDay = DateTime.DaysInMonth(currentYear, currentMonth);

            var nonIntRows = new List<ExpectedDTO>();
            int day;
            foreach (var row in expectedRows)
            {
                if (int.TryParse(row.Day, out day))
                {
                    switch (row.Recurring)
                    {
                        case ReccuringType.Monthly:
                            transactions.Add(new TransationDTO
                            {
                                Date = new DateTime(currentYear, currentMonth, day),
                                Amount = row.Amount,
                                Title = row.Title
                            });
                            break;
                        case ReccuringType.Daily:
                            for (var i = 1; i <= finalDay; i++)
                            {
                                var date = new DateTime(currentYear, currentMonth, i);
                                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    continue;
                                }
                                transactions.Add(new TransationDTO
                                {
                                    Date = new DateTime(currentYear, currentMonth, i),
                                    Amount = row.Amount,
                                    Title = row.Title
                                });
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (row.Recurring)
                    {
                        case ReccuringType.Weekly:
                            for (var i = 1; i <= finalDay; i++)
                            {
                                var date = new DateTime(currentYear, currentMonth, i);
                                if (date.DayOfWeek.ToString() == row.Day)
                                {
                                    transactions.Add(new TransationDTO
                                    {
                                        Date = new DateTime(currentYear, currentMonth, i),
                                        Amount = row.Amount,
                                        Title = row.Title
                                    });
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            transactions = transactions.OrderBy(t => t.Date).ToList();

            var total = StartingAmount;
            foreach (var transaction in transactions)
            {
                transaction.RunningTotal = total + transaction.Amount;
                total = transaction.RunningTotal;
            }
            return transactions;
        }
    }
}
