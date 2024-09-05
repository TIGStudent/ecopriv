using ecopriv.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ecopriv.UI
{
    public class WindowDragBehavior
    {

        private bool isResizing;
        private ResizeDirection resizeDirection;
        private Point initialMousePosition;
        private Rect initialWindowBounds;
        public void TitleBar_MouseLeftButtonDown(Window window, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                window.DragMove();
            }
        }
        public void ResizeHandle_MouseLeftButtonDown(Window window, object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isResizing = true;
                resizeDirection = GetResizeDirection(sender as Border);
                initialMousePosition = e.GetPosition(window);
                initialWindowBounds = new Rect(window.Left, window.Top, window.Width, window.Height);
                Mouse.Capture(sender as IInputElement);
            }
        }

        public void ResizeHandle_MouseMove(Window window, object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                var currentPosition = e.GetPosition(window);
                double deltaX = currentPosition.X - initialMousePosition.X;
                double deltaY = currentPosition.Y - initialMousePosition.Y;

                switch (resizeDirection)
                {
                    case ResizeDirection.Bottom:
                        window.Height = initialWindowBounds.Height + deltaY;
                        break;

                    case ResizeDirection.Top:
                        // Decrease height and move the window up
                        window.Height = initialWindowBounds.Height - deltaY;
                        window.Top = initialWindowBounds.Top + deltaY;
                        break;

                    case ResizeDirection.Right:
                        window.Width = initialWindowBounds.Width + deltaX;
                        break;

                    case ResizeDirection.Left:
                        // Decrease width and move the window left
                        window.Width = initialWindowBounds.Width - deltaX;
                        window.Left = initialWindowBounds.Left + deltaX;
                        break;

                    case ResizeDirection.BottomRight:
                        // Increase both height and width
                        window.Width = initialWindowBounds.Width + deltaX;
                        window.Height = initialWindowBounds.Height + deltaY;
                        break;

                    case ResizeDirection.BottomLeft:
                        // Increase height, decrease width, and move the window left
                        window.Width = initialWindowBounds.Width - deltaX;
                        window.Height = initialWindowBounds.Height + deltaY;
                        window.Left = initialWindowBounds.Left + deltaX;
                        break;

                    case ResizeDirection.TopRight:
                        // Decrease height, increase width, and move the window up
                        window.Width = initialWindowBounds.Width + deltaX;
                        window.Height = initialWindowBounds.Height - deltaY;
                        window.Top = initialWindowBounds.Top + deltaY;
                        break;

                    case ResizeDirection.TopLeft:
                        // Decrease both height and width, and move the window up and left
                        window.Width = initialWindowBounds.Width - deltaX;
                        window.Height = initialWindowBounds.Height - deltaY;
                        window.Top = initialWindowBounds.Top + deltaY;
                        window.Left = initialWindowBounds.Left + deltaX;
                        break;
                }
            }
        }

        public void ResizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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


    }
}
