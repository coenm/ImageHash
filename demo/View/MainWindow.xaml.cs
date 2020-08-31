namespace Demo.View
{
    using System.Windows;

    using Demo.ViewModel;

    public partial class MainWindow : Window
    {
        public MainWindow(DemoViewModel demoViewModel)
        {
            InitializeComponent();
            DataContext = demoViewModel;
        }
    }
}
