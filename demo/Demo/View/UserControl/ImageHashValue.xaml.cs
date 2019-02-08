using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.View.UserControl
{
    using UserControl = System.Windows.Controls.UserControl;

    /// <summary>
    /// Interaction logic for ImageHashValue.xaml
    /// </summary>
    public partial class ImageHashValue : UserControl
    {
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register("Filename", typeof(string), typeof(ImageHashValue), new PropertyMetadata(default(string)));

        public string Filename
        {
            get { return (string)GetValue(FilenameProperty); }
            set { SetValue(FilenameProperty, value); }
        }

        public ImageHashValue()
        {
            InitializeComponent();
        }
    }
}
