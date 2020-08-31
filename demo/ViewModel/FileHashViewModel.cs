namespace Demo.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    using Model;
    using Nito.Mvvm;

    public class FileHashViewModel : ViewModelBase
    {
        private readonly IFileSystem fileSystem;

        public FileHashViewModel(IDemoImageHash imageHash, IFileSystem fileSystem)
        {
            if (imageHash == null)
                throw new ArgumentNullException(nameof(imageHash));

            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));

            LoadCommand = new CapturingExceptionAsyncCommand(
                async () =>
                {
                    try
                    {
                        Busy = true;
                        var filename = FileName;
                        Image = await Task.Run(() => LoadImg(filename));
                        AverageHash = await Task.Run(() => imageHash.CalculateAverageHash(filename));
                        DifferenceHash = await Task.Run(() => imageHash.CalculateDifferenceHash(filename));
                        PerceptualHash = await Task.Run(() => imageHash.CalculatePerceptualHash(filename));
                        Loaded = true;
                    }
                    finally
                    {
                        Busy = false;
                    }
                },
                () => Busy == false && string.IsNullOrWhiteSpace(FileName) == false);

            ClearCommand = new CapturingExceptionAsyncCommand(() =>
                {
                    Initialize();
                    return Task.CompletedTask;
                },
                () => Busy == false);


            PropertyChanged += (sender, args) =>
            {
                LoadCommand.OnCanExecuteChanged();
                ClearCommand.OnCanExecuteChanged();
            };
        }

        public bool Loaded
        {
            get => Properties.Get(false);
            private set => Properties.Set(value);
        }

        public bool Busy
        {
            get => Properties.Get(false);
            set => Properties.Set(value);
        }

        public BitmapImage Image
        {
            get => Properties.Get(new BitmapImage());
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
            get => Properties.Get(string.Empty);
            set => Properties.Set(value);
        }

        public CapturingExceptionAsyncCommand LoadCommand { get; }

        public CapturingExceptionAsyncCommand ClearCommand { get; }

        private BitmapImage LoadImg(string file)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = fileSystem.OpenRead(file);
            bitmapImage.EndInit();

            // https://stackoverflow.com/questions/26361020/error-must-create-dependencysource-on-same-thread-as-the-dependencyobject-even
            bitmapImage.Freeze();

            return bitmapImage;
        }

        private void Initialize()
        {
            Loaded = false;
            Image = new BitmapImage();
            AverageHash = 0;
            DifferenceHash = 0;
            PerceptualHash = 0;
            FileName = string.Empty;
        }
    }
}
