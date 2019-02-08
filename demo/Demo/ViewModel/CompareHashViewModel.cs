namespace Demo.ViewModel
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Model;
    using Nito.Mvvm;

    public class CompareHashViewModel : ViewModelBase
    {
        public CompareHashViewModel(
            [NotNull] IImageHashSimilarityCalculator calculator,
            [NotNull] FileHashViewModel fileA,
            [NotNull] FileHashViewModel fileB)
        {
            if (calculator == null)
                throw new ArgumentNullException(nameof(calculator));

            if (fileA == null)
                throw new ArgumentNullException(nameof(fileA));

            if (fileB == null)
                throw new ArgumentNullException(nameof(fileB));

            CalculateCommand = new CapturingExceptionAsyncCommand(
                async () =>
                {
                    var f1 = fileA.AverageHash;
                    var f2 = fileB.AverageHash;
                    AverageHash = await Task.Run(() => calculator.Calculate(f1, f2));

                    f1 = fileA.DifferenceHash;
                    f2 = fileB.DifferenceHash;
                    DifferenceHash = await Task.Run(() => calculator.Calculate(f1, f2));

                    f1 = fileA.PerceptualHash;
                    f2 = fileB.PerceptualHash;
                    PerceptualHash = await Task.Run(() => calculator.Calculate(f1, f2));
                });
        }


        public double AverageHash
        {
            get => Properties.Get<double>(0);
            private set => Properties.Set(value);
        }

        public double DifferenceHash
        {
            get => Properties.Get<double>(0);
            private set => Properties.Set(value);
        }

        public double PerceptualHash
        {
            get => Properties.Get<double>(0);
            private set => Properties.Set(value);
        }

        public IAsyncCommand CalculateCommand { get; }
    }
}
