namespace Demo.ViewModel
{
    using System.IO;
    using System.Windows.Input;
    using CoenM.ImageHash;
    using CoenM.ImageHash.HashAlgorithms;
    using JetBrains.Annotations;
    using Microsoft.Expression.Interactivity.Core;


    public class FileHashResultsViewModel : ViewModelBase
    {
        [NotNull] private readonly IImageHash imageHash;

        public FileHashResultsViewModel([NotNull] IImageHash imageHash)
        {
            this.imageHash = imageHash;
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
    }

    public class DemoViewModel : ViewModelBase
    {
        [NotNull] private readonly AverageHash averageHash;

        public DemoViewModel()
        {
            averageHash = new AverageHash();
            ulong result;
            Calculate = new ActionCommand(() => result = averageHash.Hash(new MemoryStream()));
        }

        public string FileA
        {
            get => Properties.Get<string>(string.Empty);
            set => Properties.Set(value);
        }
        public string FirstFilename
        {
            get => Properties.Get<string>(string.Empty);
            set => Properties.Set(value);
        }

        public string FileB
        {
            get => Properties.Get<string>(string.Empty);
            set => Properties.Set(value);
        }

        public ICommand Calculate { get; }
    }
}
