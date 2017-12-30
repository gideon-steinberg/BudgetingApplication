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
        private Dictionary<object, string> TextInputCache = new Dictionary<object, string>();

        public MainWindow()
        {
            InitializeComponent();
            _expectedRows = _db.ReadExpectedValuesFromDatabase();

            ResetCurrentMonthComboBox();
            YearTextBox.SelectionChanged += ResetValues;
            CurrentMonthComboBox.SelectionChanged += ResetValues;
            StartingAmountTextBox.SelectionChanged += ResetValues;
            SetNumberListeners();

            ResetValues(null, null);
        }

        private void SetNumberListeners()
        {
            YearTextBox.PreviewTextInput += EnforceNumbers;
            YearTextBox.TextChanged += ResetIfInvalid;
            YearTextBox.AcceptsTab = false;
            YearTextBox.AcceptsReturn = false;

            StartingAmountTextBox.PreviewTextInput += EnforceNumbers;
            StartingAmountTextBox.TextChanged += ResetIfInvalid;
            StartingAmountTextBox.AcceptsTab = false;
            StartingAmountTextBox.AcceptsReturn = false;
        }

        private void ResetIfInvalid(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                (sender as TextBox).Text = "0";
            }

            if (TextInputCache.ContainsKey(sender))
            {
                int i;
                if (!int.TryParse((sender as TextBox).Text, out i))
                {
                    (sender as TextBox).Text = TextInputCache[sender];
                }
                TextInputCache.Remove(sender);
            }
        }

        private void EnforceNumbers(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "-")
            {
                TextInputCache[sender] = (sender as TextBox).Text;
                return;
            }
            int i;
            e.Handled = !int.TryParse(e.Text, out i);
        }

        private void ResetCurrentMonthComboBox()
        {
            CurrentMonthComboBox.Items.Clear();
            CurrentMonthComboBox.Items.Add("Jan");
            CurrentMonthComboBox.Items.Add("Feb");
            CurrentMonthComboBox.Items.Add("Mar");
            CurrentMonthComboBox.Items.Add("Apr");
            CurrentMonthComboBox.Items.Add("May");
            CurrentMonthComboBox.Items.Add("Jun");
            CurrentMonthComboBox.Items.Add("Jul");
            CurrentMonthComboBox.Items.Add("Aug");
            CurrentMonthComboBox.Items.Add("Sep");
            CurrentMonthComboBox.Items.Add("Oct");
            CurrentMonthComboBox.Items.Add("Nov");
            CurrentMonthComboBox.Items.Add("Dec");

            CurrentMonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        private int GetSelectedMonth() => CurrentMonthComboBox.SelectedIndex + 1;
        private int GetSelectedYear()
        {
            var year = int.Parse(YearTextBox.Text);
            if (year <=1 || year > 4000)
            {
                YearTextBox.Text = DateTime.Now.Year.ToString();
                return DateTime.Now.Year;
            }
            return year;
        }

        private int GetStartingAmount() => int.Parse(StartingAmountTextBox.Text);

        private void ResetValues(object o, object s)
        {
            var transactions = CalculatorService.CalculateTransactions(_expectedRows, GetStartingAmount(), GetSelectedMonth(), GetSelectedYear());

            var builder = new StringBuilder();
            foreach(var trans in transactions)
            {
                builder.Append($"{trans.Date.ToShortDateString()} : ({trans.RunningTotal}) {trans.Title} {trans.Amount}\n");
            }
            TransactionList.Text = builder.ToString();

            DrawGraph(transactions);

            _db.WriteExpectedValuesToDatabase(_expectedRows);
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

            var currentYear = GetSelectedYear();
            var currentMonth = GetSelectedMonth();

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
