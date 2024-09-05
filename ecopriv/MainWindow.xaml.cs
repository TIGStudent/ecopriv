using ecopriv.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ecopriv.UI;

namespace ecopriv
{
    public partial class MainWindow : Window
    {

        private WindowDragBehavior _windowDragBehavior;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel();
            _windowDragBehavior = new WindowDragBehavior();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _windowDragBehavior.TitleBar_MouseLeftButtonDown(this, e);
        }
        private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
        {
            _windowDragBehavior.ResizeHandle_MouseMove(this, sender, e);
        }
        private void ResizeHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _windowDragBehavior.ResizeHandle_MouseLeftButtonDown(this, sender, e);
        }

        private void ResizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _windowDragBehavior.ResizeHandle_MouseLeftButtonUp(sender, e);
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle visibility of AddBox
            if (AddBox.Visibility == Visibility.Collapsed)
            {
                AddBox.Visibility = Visibility.Visible;
                AddButton.Content = "Close Add";
            }
            else
            {
                AddBox.Visibility = Visibility.Collapsed;
                AddButton.Content = "Open Add";
            }
        }
        private void AddSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve values from the TextBoxes
            var typeTextBox = (TextBox)AddBox.FindName("TypeTextBox");
            var valueTextBox = (TextBox)AddBox.FindName("ValueTextBox");
            var assignmentTextBox = (TextBox)AddBox.FindName("AssignmentTextBox");

            if (typeTextBox == null || valueTextBox == null || assignmentTextBox == null)
            {
                MessageBox.Show("One or more text boxes could not be found.");
                return;
            }

            string type = typeTextBox.Text;
            decimal value;

            if (!decimal.TryParse(valueTextBox.Text, out value))
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }
            string assignment = assignmentTextBox.Text;
            int amountAsInt = (int)value;

            // Create a new Sum object and add it to the Numbers collection
            var newSum = new Sum(type, amountAsInt, assignment);
            var viewModel = (ViewModel)DataContext;
            viewModel.Numbers.Add(newSum);

            // Print to the console
            Console.WriteLine($"Added: Type = {newSum.Type}, Value = {newSum.Value}, Assignment = {newSum.Assignment}");

            // Clear the TextBoxes
            typeTextBox.Text = string.Empty;
            valueTextBox.Text = string.Empty;
            assignmentTextBox.Text = string.Empty;

            // Hide the AddBox
            AddBox.Visibility = Visibility.Collapsed;
            AddButton.Content = "Open Add";
        }
    }
}
