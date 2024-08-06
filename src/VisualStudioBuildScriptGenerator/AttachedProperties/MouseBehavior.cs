using System.Windows;
using System.Windows.Input;

namespace VisualStudioBuildScriptGenerator
{
    static class MouseBehavior
    {
        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(MouseBehavior), new PropertyMetadata(MouseDownCommandChanged));

        public static void SetMouseDownCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(MouseDownCommandProperty, value);
        }

        public static ICommand GetMouseDownCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(MouseDownCommandProperty);
        }

        private static void MouseDownCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement element)
            {
                element.MouseDown -= Element_MouseDown;
                if (e.NewValue is ICommand)
                {
                    element.MouseDown += Element_MouseDown;
                }
            }
        }

        private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as UIElement;
            var command = GetMouseDownCommand(element);
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }

        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.RegisterAttached("MouseMoveCommand", typeof(ICommand), typeof(MouseBehavior), new PropertyMetadata(MouseMoveCommandChanged));

        public static void SetMouseMoveCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(MouseMoveCommandProperty, value);
        }

        public static ICommand GetMouseMoveCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(MouseMoveCommandProperty);
        }

        private static void MouseMoveCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement element)
            {
                element.MouseMove -= Element_MouseMove;
                if (e.NewValue is ICommand)
                {
                    element.MouseMove += Element_MouseMove;
                }
            }
        }

        private static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            var element = sender as UIElement;
            var command = GetMouseMoveCommand(element);
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }

        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand), typeof(MouseBehavior), new PropertyMetadata(MouseUpCommandChanged));

        public static void SetMouseUpCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(MouseUpCommandProperty, value);
        }

        public static ICommand GetMouseUpCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(MouseUpCommandProperty);
        }

        private static void MouseUpCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is UIElement element)
            {
                element.MouseUp -= Element_MouseUp;
                if (e.NewValue is ICommand)
                {
                    element.MouseUp += Element_MouseUp;
                }
            }
        }

        private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as UIElement;
            var command = GetMouseUpCommand(element);
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }
    }
}
