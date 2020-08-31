namespace Demo.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Model;
    using Nito.Mvvm;

    public class CompareHashViewModel : ViewModelBase, IDisposable
    {
        private readonly FileHashViewModel fileA;
        private readonly FileHashViewModel fileB;

        public CompareHashViewModel(
            IImageHashSimilarityCalculator calculator,
            FileHashViewModel fileA,
            FileHashViewModel fileB)
        {
            if (calculator == null)
                throw new ArgumentNullException(nameof(calculator));

            this.fileA = fileA ?? throw new ArgumentNullException(nameof(fileA));
            this.fileB = fileB ?? throw new ArgumentNullException(nameof(fileB));

            CalculateCommand = new CapturingExceptionAsyncCommand(
                async () =>
                {
                    Busy = true;

                    try
                    {
                        var a1 = fileA.AverageHash;
                        var a2 = fileB.AverageHash;
                        AverageHash = await Task.Run(() => calculator.Calculate(a1, a2));

                        var d1 = fileA.DifferenceHash;
                        var d2 = fileB.DifferenceHash;
                        DifferenceHash = await Task.Run(() => calculator.Calculate(d1, d2));

                        var p1 = fileA.PerceptualHash;
                        var p2 = fileB.PerceptualHash;
                        PerceptualHash = await Task.Run(() => calculator.Calculate(p1, p2));
                    }
                    finally
                    {
                        Busy = false;
                    }
                },
                () => fileA.Loaded && fileB.Loaded && !Busy);

            PropertyChanged += OnPropertyChanged;
            fileA.PropertyChanged += OnPropertyChanged;
            fileB.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculateCommand.OnCanExecuteChanged();
        }

        public bool Busy
        {
            get => Properties.Get<bool>(false);
            private set => Properties.Set(value);
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

        public CapturingExceptionAsyncCommand CalculateCommand { get; }

        public void Dispose()
        {
            PropertyChanged -= OnPropertyChanged;
            fileA.PropertyChanged -= OnPropertyChanged;
            fileB.PropertyChanged -= OnPropertyChanged;
        }
    }
}
