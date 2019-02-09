using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Demo
{
    using Model;
    using ViewModel;
    using MainWindow = View.MainWindow;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var fileSystem = new FileSystem();

                var vm = new DemoViewModel(fileSystem,
                    new ImageHashFacade(fileSystem), new ImageHashSimilarityCalculator());

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
