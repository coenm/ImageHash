namespace Demo.View.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Xaml.Behaviors;
    using Ookii.Dialogs.Wpf;

    public class FolderDialogBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty FolderName = DependencyProperty.RegisterAttached("FolderName", typeof(string), typeof(FolderDialogBehavior));

        public static string GetFolderName(DependencyObject obj)
        {
            return (string)obj.GetValue(FolderName);
        }

        public static void SetFolderName(DependencyObject obj, string value)
        {
            obj.SetValue(FolderName, value);
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
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Please select a folder.",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
            };

            var path = (string)GetValue(FolderName);
            dialog.SelectedPath = path;

            if ((bool)dialog.ShowDialog(null))
                SetValue(FolderName, dialog.SelectedPath);
        }
    }
}
