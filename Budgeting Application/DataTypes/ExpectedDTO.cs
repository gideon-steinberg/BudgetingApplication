using Budgeting_Application.DataTypes;
using System;

namespace Budgeting_Application.Services
{
    public class ExpectedDTO
    {
        public string Title { get; set; }
        public int Amount { get; set; }
        public DateTime DateStarted { get; set; }
        public ReccuringType Recurring { get; set; }
    }
}
