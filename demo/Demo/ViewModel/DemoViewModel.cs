namespace Demo.ViewModel
{
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    using CoenM.ImageHash;
    using CoenM.ImageHash.HashAlgorithms;
    using JetBrains.Annotations;
    using Model;
    using Nito.Mvvm;

    public class DemoViewModel : ViewModelBase
    {
        [NotNull] private readonly IFileSystem fileSystem;
        [NotNull] private readonly AverageHash averageHash;

        public DemoViewModel([NotNull] IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            averageHash = new AverageHash();

            FirstFilename = string.Empty;
            Calculate = new CapturingExceptionAsyncCommand(async () =>
            {
                var filename = FirstFilename;
                FileA = await Task.Run(() => LoadImg(filename));
                AverageHashFile1 = await Task.Run(() => averageHash.Hash(fileSystem.OpenRead(filename)));
            });
        }

        private BitmapImage LoadImg(string file)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = fileSystem.OpenRead(file);
            img.EndInit();

            // https://stackoverflow.com/questions/26361020/error-must-create-dependencysource-on-same-thread-as-the-dependencyobject-even
            img.Freeze();

            return img;
        }

        public ulong AverageHashFile1
        {
            get => Properties.Get<ulong>(0);
            set => Properties.Set(value);
        }

        public BitmapImage FileA
        {
            get => Properties.Get<BitmapImage>(new BitmapImage());
            set => Properties.Set(value);
        }

        public string FirstFilename
        {
            get => Properties.Get<string>(string.Empty);
            set => Properties.Set(value);
        }

        public BitmapImage FileB
        {
            get => Properties.Get<BitmapImage>(new BitmapImage());
            set => Properties.Set(value);
        }

        public IAsyncCommand Calculate { get; }
    }
}
