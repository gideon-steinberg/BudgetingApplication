using System;

namespace Budgeting_Application.DataTypes
{
    public class TransationDTO
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int RunningTotal { get; set; }
        public int Amount { get; set; }
    }
}
