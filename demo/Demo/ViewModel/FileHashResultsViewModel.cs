namespace Demo.ViewModel
{
    using CoenM.ImageHash;
    using JetBrains.Annotations;

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
}