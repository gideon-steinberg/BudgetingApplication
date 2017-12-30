using Budgeting_Application.DataTypes;
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
            var transactions = CalculatorService.CalculateTransactions(_expectedRows);

            var builder = new StringBuilder();
            foreach(var trans in transactions)
            {
                builder.Append($"{trans.Date.ToShortDateString()} : ({trans.RunningTotal}) {trans.Title} {trans.Amount}\n");
            }
            TransactionList.Text = builder.ToString();

            DrawGraph(transactions);
        }

        private void DrawGraph(List<TransationDTO> transactions)
        {
            Graph.Children.Clear();

            // Title
            var text = new TextBlock();
            text.Text = "Graph";
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 0);
            Canvas.SetTop(text, 0);
            Graph.Children.Add(text);

            var line = new Line();
            line.Stroke = Brushes.Black;

            line.X1 = 65;
            line.X2 = 65;
            line.Y1 = 50;
            line.Y2 = 450;

            line.StrokeThickness = 2;

            Graph.Children.Add(line);

            line = new Line();
            line.Stroke = Brushes.Black;
            line.X1 = 65;
            line.X2 = 375;
            line.Y1 = 450;
            line.Y2 = 450;
            line.StrokeThickness = 2;
            Graph.Children.Add(line);

            var maxValue = transactions.Max(trans => trans.RunningTotal);
            var minValue = transactions.Min(trans => trans.RunningTotal);

            text = new TextBlock();
            text.Text = maxValue.ToString();
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 10);
            Canvas.SetTop(text, 50);
            Graph.Children.Add(text);

            text = new TextBlock();
            text.Text = minValue.ToString();
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 10);
            Canvas.SetTop(text, 425);
            Graph.Children.Add(text);

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            text = new TextBlock();
            text.Text = $"1/{currentMonth}/{currentYear}";
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 50);
            Canvas.SetTop(text, 475);
            Graph.Children.Add(text);

            var finalDay = DateTime.DaysInMonth(currentYear, currentMonth);

            text = new TextBlock();
            text.Text = $"{finalDay}/{currentMonth}/{currentYear}";
            text.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(text, 325);
            Canvas.SetTop(text, 475);
            Graph.Children.Add(text);

            var xPixelsPerDay = 300.0 / finalDay;
            var yPixelsPerAmount = 385.0 / (maxValue - minValue);

            var previousXCoordinate = -1000;
            var previousYCoordinate = -1000;
            foreach (var transaction in transactions)
            {
                var currentX = (int)(65 + (transaction.Date.Day) * xPixelsPerDay);
                var currentY = (int)(50 + (maxValue - transaction.RunningTotal) * yPixelsPerAmount);

                if (previousXCoordinate > 0)
                {
                    line = new Line();
                    line.Stroke = Brushes.LightBlue;
                    line.X1 = previousXCoordinate;
                    line.X2 = currentX;
                    line.Y1 = previousYCoordinate;
                    line.Y2 = currentY;
                    line.StrokeThickness = 2;
                    Graph.Children.Add(line);
                }

                var point = new Ellipse();
                point.Fill = new SolidColorBrush(Colors.Green);
                point.StrokeThickness = 2;
                point.Stroke = Brushes.Green;
                point.Width = 4;
                point.Height = 4;
                Canvas.SetLeft(point, currentX - 2);
                Canvas.SetTop(point, currentY - 2);

                Graph.Children.Add(point);

                previousXCoordinate = currentX;
                previousYCoordinate = currentY;
            }
        }
    }
}
