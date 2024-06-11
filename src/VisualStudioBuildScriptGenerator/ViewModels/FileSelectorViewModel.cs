using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VisualStudioBuildScriptGenerator
{
    internal class FileSelectorViewModel : ViewModelBase
    {
        private Point startPoint;
        private bool isDragging = false;
        private Canvas canvas;
        private Rectangle selectionRectangle;
        private Visibility selectionRectVisibility = Visibility.Collapsed;

        public ObservableCollection<string> Items { get; set; }
        public ICommand MouseDownCommand { get; }
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseUpCommand { get; }

        public FileSelectorViewModel()
        {
            Items = new ObservableCollection<string> {
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" ,
            };
            MouseDownCommand = new RelayCommand<object>(OnMouseDown);
            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove);
            MouseUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseUp);
        }

        public Visibility SelectionRectVisibility { get; set; }

        private void OnMouseDown(object parameter)
        {
            if (parameter is MouseButtonEventArgs e && e.ChangedButton == MouseButton.Left)
            {
                if (e.Source is FrameworkElement fe)
                {
                    startPoint = e.GetPosition(fe);
                    isDragging = true;

                    canvas = FindParent<Canvas>(fe);
                    selectionRectangle = (Rectangle)canvas.FindName("selectionRectangle");
                    if (canvas != null && selectionRectangle != null)
                    {
                        Canvas.SetLeft(selectionRectangle, startPoint.X);
                        Canvas.SetTop(selectionRectangle, startPoint.Y);
                        selectionRectangle.Width = 0;
                        selectionRectangle.Height = 0;
                        SelectionRectVisibility = Visibility.Visible;
                    }

                    if (fe is ListBox listBox)
                    {
                        listBox.UnselectAll();
                    }
                }
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(canvas);

                double x = Math.Min(startPoint.X, currentPoint.X);
                double y = Math.Min(startPoint.Y, currentPoint.Y);
                double width = Math.Abs(startPoint.X - currentPoint.X);
                double height = Math.Abs(startPoint.Y - currentPoint.Y);

                Canvas.SetLeft(selectionRectangle, x);
                Canvas.SetTop(selectionRectangle, y);
                selectionRectangle.Width = width;
                selectionRectangle.Height = height;

                SelectItemsWithinRect(new Rect(x, y, width, height));
            }
        }

        private void OnMouseUp(MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
                SelectionRectVisibility = Visibility.Collapsed;
            }
        }

        private void SelectItemsWithinRect(Rect selectionRect)
        {
            if (canvas == null) return;

            foreach (var item in ((ListBox)canvas.Children[0]).Items)
            {
                ListBoxItem listBoxItem = ((ListBox)canvas.Children[0]).ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (listBoxItem != null)
                {
                    Rect itemRect = new Rect(
                        listBoxItem.TranslatePoint(new Point(), canvas),
                        listBoxItem.RenderSize);

                    if (selectionRect.IntersectsWith(itemRect))
                    {
                        listBoxItem.IsSelected = true;
                    }
                }
            }
        }
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            while (parentObject != null && !(parentObject is T))
            {
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }
            return parentObject as T;
        }
    }
}
