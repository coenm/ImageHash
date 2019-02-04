namespace Demo.ViewModel
{
    using System.IO;
    using System.Windows.Input;
    using CoenM.ImageHash;
    using CoenM.ImageHash.HashAlgorithms;
    using JetBrains.Annotations;
    using Microsoft.Expression.Interactivity.Core;


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
