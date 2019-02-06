namespace Demo.ViewModel
{
    using System;
    using JetBrains.Annotations;
    using Model;

    public class DemoViewModel : ViewModelBase
    {
        public DemoViewModel([NotNull] IDemoImageHash imageHash, [NotNull] IFileSystem fileSystem)
        {
            if (imageHash == null)
              throw new ArgumentNullException(nameof(imageHash));

            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            FileA = new FileHashViewModel(imageHash, fileSystem);
            FileB = new FileHashViewModel(imageHash, fileSystem);
        }

        public FileHashViewModel FileA { get; }

        public FileHashViewModel FileB { get; }
    }
}
