using Budgeting_Application.DataTypes;
using Budgeting_Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Budgeting_Application
{
    /// <summary>
    /// Interaction logic for ExpectedValues.xaml
    /// </summary>
    public partial class ExpectedValues : Window
    {
        private Database _db = new Database();

        public ExpectedValues()
        {
            InitializeComponent();
            var expectedValues = _db.ReadExpectedValuesFromDatabase();
            Table.CanUserAddRows = true;
            Table.CanUserDeleteRows = true;
            var list = new ObservableCollection<ExpectedDTO>(expectedValues);
            Table.ItemsSource = list;
            SaveChangesButton.Click += SaveChangesButton_Click;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
                var toSave = new List<ExpectedDTO>();
                int i;
                foreach(var item in Table.Items)
                {
                    var expectedItem = item as ExpectedDTO;
                    if (expectedItem == null)
                    {
                        continue;
                    }
                    switch (expectedItem.Recurring)
                    {
                        case ReccuringType.Yearly:
                            Convert.ToDateTime($"{expectedItem.Day} {DateTime.Now.Year}");
                            break;
                        case ReccuringType.Monthly:
                            if (!(int.TryParse(expectedItem.Day, out i) || expectedItem.Day == "EndOfMonth"))
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case ReccuringType.BiMonthly:
                            if (!int.TryParse(expectedItem.Day, out i))
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case ReccuringType.Weekly:
                            if (Enum.Parse(typeof(DayOfWeek), expectedItem.Day) == null)
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            break;    
                    }
                    toSave.Add(expectedItem);
                }
                _db.WriteExpectedValuesToDatabase(toSave);
            }
            catch (Exception)
            {
                MessageBox.Show("There was some invalid data so it was not saved");
            }
        }
    }
}
