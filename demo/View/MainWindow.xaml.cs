namespace Demo.View
{
    using System.Windows;

    using JetBrains.Annotations;
    using ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow([NotNull] DemoViewModel demoViewModel)
        {
            InitializeComponent();
            DataContext = demoViewModel;
        }
    }
}
