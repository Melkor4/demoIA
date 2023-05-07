using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeuralNetworkDemo.Library.Objects
{
    public class Neuron : INotifyPropertyChanged
    {

        #region Public Constructors

        public Neuron()
        {
        }

        #endregion

        #region Fields

        private double currentValue = 0;
        private bool isHumanInput = false;
        private string? label;

        #endregion

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties + Indexers

        public double CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsHumanInput { get => isHumanInput; init => isHumanInput = value; }

        public string? Label
        {
            get => label;
            set
            {
                label = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

    }
}
