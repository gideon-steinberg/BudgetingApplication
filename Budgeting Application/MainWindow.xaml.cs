using Budgeting_Application.Services;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Budgeting_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ExpectedDTO> _expectedRows;
        private Database _db = new Database();

        public MainWindow()
        {
            InitializeComponent();
            _expectedRows = _db.ReadExpectedValuesFromDatabase();
            ResetValues();
        }

        private void ResetValues()
        {
            var builder = new StringBuilder();
            foreach(var row in _expectedRows.OrderBy(r => r.Day))
            {
                builder.Append($"{row.Day} : {row.Title} - {row.Amount}, {row.Recurring}\n");
            }
            TransactionList.Text = builder.ToString();

            var line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = 1;
            line.X2 = 375;
            line.Y1 = 1;
            line.Y2 = 500;

            line.StrokeThickness = 2;
            Graph.Children.Clear();
            Graph.Children.Add(line);

            var text = new TextBlock();
            text.Text = "Graph";
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 0);
            Canvas.SetTop(text, 0);
            Graph.Children.Add(text);

        }
    }
}
