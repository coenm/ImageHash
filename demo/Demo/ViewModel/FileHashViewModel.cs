namespace Demo.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using JetBrains.Annotations;
    using Model;
    using Nito.Mvvm;

    public class FileHashViewModel : ViewModelBase
    {
        [NotNull] private readonly IDemoImageHash imageHash;
        [NotNull] private readonly IFileSystem fileSystem;

        public FileHashViewModel([NotNull] IDemoImageHash imageHash, [NotNull] IFileSystem fileSystem)
        {
            this.imageHash = imageHash ?? throw new ArgumentNullException(nameof(imageHash));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            LoadCommand = new CapturingExceptionAsyncCommand(
                async () =>
                {
                    var filename = FileName;
                    Image = await Task.Run(() => LoadImg(filename));
                    AverageHash = await Task.Run(() => this.imageHash.CalculateAverageHash(filename));
                    DifferenceHash = await Task.Run(() => this.imageHash.CalculateDifferenceHash(filename));
                    PerceptualHash = await Task.Run(() => this.imageHash.CalculatePerceptualHash(filename));
                });
        }

        public BitmapImage Image
        {
            get => Properties.Get<BitmapImage>(new BitmapImage());
            set => Properties.Set(value);
        }

        public ulong AverageHash
        {
            get => Properties.Get<ulong>(0);
            private set => Properties.Set(value);
        }

        public ulong DifferenceHash
        {
            get => Properties.Get<ulong>(0);
            private set => Properties.Set(value);
        }

        public ulong PerceptualHash
        {
            get => Properties.Get<ulong>(0);
            private set => Properties.Set(value);
        }

        public string FileName
        {
            get => Properties.Get<string>(string.Empty);
            set => Properties.Set(value);
        }

        public IAsyncCommand LoadCommand { get; }

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
    }
}
