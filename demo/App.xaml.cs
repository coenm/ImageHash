namespace Demo
{
    using System;
    using System.Windows;

    using Demo.Model;
    using Demo.ViewModel;

    using MainWindow = Demo.View.MainWindow;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var fileSystem = new FileSystem();
                var imageHashFacade = new ImageHashFacade(fileSystem);
                var imageHashSimilarityCalculator = new ImageHashSimilarityCalculator();
                var vm = new DemoViewModel(fileSystem, imageHashFacade, imageHashSimilarityCalculator);

                var view = new MainWindow(vm);
                view.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
