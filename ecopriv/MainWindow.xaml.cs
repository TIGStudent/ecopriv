using ecopriv.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ecopriv
{
    public partial class MainWindow : Window
    {
        private bool isResizing;
        private ResizeDirection resizeDirection;
        private Point initialMousePosition;
        private Rect initialWindowBounds;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResizeHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isResizing = true;
                resizeDirection = GetResizeDirection(sender as Border);
                initialMousePosition = e.GetPosition(this);
                initialWindowBounds = new Rect(this.Left, this.Top, this.Width, this.Height);
                Mouse.Capture(sender as IInputElement);
            }
        }

        private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                var currentPosition = e.GetPosition(this);
                double deltaX = currentPosition.X - initialMousePosition.X;
                double deltaY = currentPosition.Y - initialMousePosition.Y;

                switch (resizeDirection)
                {
                    case ResizeDirection.Bottom:
                        Height = initialWindowBounds.Height + deltaY;
                        break;

                    case ResizeDirection.Top:
                        // Decrease height and move the window up
                        Height = initialWindowBounds.Height - deltaY;
                        Top = initialWindowBounds.Top + deltaY;
                        break;

                    case ResizeDirection.Right:
                        Width = initialWindowBounds.Width + deltaX;
                        break;

                    case ResizeDirection.Left:
                        // Decrease width and move the window left
                        Width = initialWindowBounds.Width - deltaX;
                        Left = initialWindowBounds.Left + deltaX;
                        break;

                    case ResizeDirection.BottomRight:
                        // Increase both height and width
                        Width = initialWindowBounds.Width + deltaX;
                        Height = initialWindowBounds.Height + deltaY;
                        break;

                    case ResizeDirection.BottomLeft:
                        // Increase height, decrease width, and move the window left
                        Width = initialWindowBounds.Width - deltaX;
                        Height = initialWindowBounds.Height + deltaY;
                        Left = initialWindowBounds.Left + deltaX;
                        break;

                    case ResizeDirection.TopRight:
                        // Decrease height, increase width, and move the window up
                        Width = initialWindowBounds.Width + deltaX;
                        Height = initialWindowBounds.Height - deltaY;
                        Top = initialWindowBounds.Top + deltaY;
                        break;

                    case ResizeDirection.TopLeft:
                        // Decrease both height and width, and move the window up and left
                        Width = initialWindowBounds.Width - deltaX;
                        Height = initialWindowBounds.Height - deltaY;
                        Top = initialWindowBounds.Top + deltaY;
                        Left = initialWindowBounds.Left + deltaX;
                        break;
                }
            }
        }



        private void ResizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isResizing)
            {
                isResizing = false;
                Mouse.Capture(null);
            }
        }

        private ResizeDirection GetResizeDirection(Border border)
        {
            if (border == null)
                return ResizeDirection.None;

            var cursor = border.Cursor;
            var horizontalAlignment = border.HorizontalAlignment;
            var verticalAlignment = border.VerticalAlignment;

            if (cursor == Cursors.SizeNS)
            {
                if (verticalAlignment == VerticalAlignment.Bottom)
                    return ResizeDirection.Bottom;
                if (verticalAlignment == VerticalAlignment.Top)
                    return ResizeDirection.Top;
            }
            else if (cursor == Cursors.SizeWE)
            {
                if (horizontalAlignment == HorizontalAlignment.Right)
                    return ResizeDirection.Right;
                if (horizontalAlignment == HorizontalAlignment.Left)
                    return ResizeDirection.Left;
            }
            else if (cursor == Cursors.SizeNWSE)
            {
                if (horizontalAlignment == HorizontalAlignment.Right && verticalAlignment == VerticalAlignment.Bottom)
                    return ResizeDirection.BottomRight;
                if (horizontalAlignment == HorizontalAlignment.Left && verticalAlignment == VerticalAlignment.Top)
                    return ResizeDirection.TopLeft;
                if (horizontalAlignment == HorizontalAlignment.Right && verticalAlignment == VerticalAlignment.Top)
                    return ResizeDirection.TopRight;
                if (horizontalAlignment == HorizontalAlignment.Left && verticalAlignment == VerticalAlignment.Bottom)
                    return ResizeDirection.BottomLeft;
            }
            else if (cursor == Cursors.SizeNESW)
            {
                if (horizontalAlignment == HorizontalAlignment.Left && verticalAlignment == VerticalAlignment.Bottom)
                    return ResizeDirection.BottomLeft;
                if (horizontalAlignment == HorizontalAlignment.Right && verticalAlignment == VerticalAlignment.Top)
                    return ResizeDirection.TopRight;
                if (horizontalAlignment == HorizontalAlignment.Left && verticalAlignment == VerticalAlignment.Top)
                    return ResizeDirection.TopLeft;
                if (horizontalAlignment == HorizontalAlignment.Right && verticalAlignment == VerticalAlignment.Bottom)
                    return ResizeDirection.BottomRight;
            }

            return ResizeDirection.None;
        }



        private enum ResizeDirection
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
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
