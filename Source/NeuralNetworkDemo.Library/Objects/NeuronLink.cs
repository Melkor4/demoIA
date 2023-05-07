using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeuralNetworkDemo.Library.Objects
{
    public class NeuronLink : INotifyPropertyChanged
    {

        #region Fields

        private double weigth = 0;

        #endregion

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties + Indexers

        public Predicate<Neuron>? ActivationFunction { get; set; }

        public Neuron? LeftNeuron { get; init; }

        public Neuron? RightNeuron { get; init; }

        public double Weigth
        {
            get => weigth;
            set
            {
                weigth = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

    }
}
