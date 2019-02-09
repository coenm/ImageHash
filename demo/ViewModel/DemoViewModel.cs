namespace Demo.ViewModel
{
    using System;
    using JetBrains.Annotations;
    using Model;

    public class DemoViewModel : ViewModelBase
    {
        public DemoViewModel([NotNull] IFileSystem fileSystem,
            [NotNull] IDemoImageHash imageHash,
            [NotNull] IImageHashSimilarityCalculator calculator)
        {
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            if (imageHash == null)
              throw new ArgumentNullException(nameof(imageHash));

            if (calculator == null)
                throw new ArgumentNullException(nameof(calculator));

            FileA = new FileHashViewModel(imageHash, fileSystem);
            FileB = new FileHashViewModel(imageHash, fileSystem);
            Compare = new CompareHashViewModel(calculator, FileA, FileB);
        }

        public FileHashViewModel FileA { get; }

        public FileHashViewModel FileB { get; }

        public CompareHashViewModel Compare { get; }
    }
}
