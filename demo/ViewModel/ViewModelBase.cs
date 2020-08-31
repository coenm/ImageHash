﻿namespace Demo.ViewModel
{
    using System.ComponentModel;

    using Nito.Mvvm.CalculatedProperties;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
#pragma warning disable SA1401 // Fields must be private
        protected readonly PropertyHelper Properties;
#pragma warning restore SA1401 // Fields must be private

        protected ViewModelBase()
        {
            Properties = new PropertyHelper(RaisePropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
