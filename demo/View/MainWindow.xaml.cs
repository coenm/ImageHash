namespace Demo.View
{
    using System.Windows;

    using ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(DemoViewModel demoViewModel)
        {
            InitializeComponent();
            DataContext = demoViewModel;
        }
    }
}
