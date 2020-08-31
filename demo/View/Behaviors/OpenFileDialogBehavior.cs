namespace Demo.View.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Xaml.Behaviors;
    using Ookii.Dialogs.Wpf;

    public class OpenFileDialogBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty FileName = DependencyProperty.RegisterAttached("FileName", typeof(string), typeof(OpenFileDialogBehavior));
        public static readonly DependencyProperty Title = DependencyProperty.RegisterAttached("Title", typeof(string), typeof(OpenFileDialogBehavior));

        public static string GetFileName(DependencyObject obj)
        {
            return (string)obj.GetValue(FileName);
        }

        public static void SetFileName(DependencyObject obj, string value)
        {
            obj.SetValue(FileName, value);
        }

        public static string GetTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(Title);
        }

        public static void SetTitle(DependencyObject obj, string value)
        {
            obj.SetValue(Title, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnClick;
            base.OnDetaching();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var filename = (string)GetValue(FileName);
            var title = (string)GetValue(Title);

            if (string.IsNullOrEmpty(title))
                title = "Select file";

            var dialog = new VistaOpenFileDialog
            {
                FileName = filename,
                DefaultExt = "jpg",
                Title = title,
            };

            bool? result = dialog.ShowDialog(null);
            if (result.HasValue && result.Value)
                SetValue(FileName, dialog.FileName);
        }
    }
}
