using Budgeting_Application.DataTypes;

namespace Budgeting_Application.DataTypes
{
    public class ExpectedDTO
    {
        public string Title { get; set; }
        public int Amount { get; set; }
        public ReccuringType Recurring { get; set; }
        public string Day { get; set; }
    }
}
