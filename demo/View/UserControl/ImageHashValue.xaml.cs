namespace Demo.View.UserControl
{
    using System.Windows;

    using UserControl = System.Windows.Controls.UserControl;

    public partial class ImageHashValue : UserControl
    {
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register("Filename", typeof(string), typeof(ImageHashValue), new PropertyMetadata(default(string)));

        public ImageHashValue()
        {
            InitializeComponent();
        }

        public string Filename
        {
            get { return (string)GetValue(FilenameProperty); }
            set { SetValue(FilenameProperty, value); }
        }
    }
}
