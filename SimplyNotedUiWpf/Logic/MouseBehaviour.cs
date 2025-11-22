using System.Windows;
using System.Windows.Input;

namespace SimplyNotedUiWpf.Logic
{
    internal class MouseBehaviour
    {
        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand),
            typeof(MouseBehaviour), new FrameworkPropertyMetadata(
            new PropertyChangedCallback(MouseDownCommandChanged)));

        private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;

            element.MouseDown += new MouseButtonEventHandler(element_MouseDown);
        }

        static void element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            ICommand command = GetMouseDownCommand(element);

            command.Execute(e);
        }

        public static void SetMouseDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseDownCommandProperty, value);
        }

        public static ICommand GetMouseDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseDownCommandProperty);
        }
    }
}