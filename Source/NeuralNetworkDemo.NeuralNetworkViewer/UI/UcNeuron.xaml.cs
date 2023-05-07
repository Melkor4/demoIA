using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NeuralNetworkDemo.Library.Objects;

namespace NeuralNetworkDemo.NeuralNetworkViewer.UI
{
    /// <summary>
    /// Interaction logic for UcNeuron.xaml
    /// </summary>
    public partial class UcNeuron : UserControl
    {

        #region Public Constructors

        public UcNeuron()
        {
            InitializeComponent();
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty NeuronBackgroundProperty = DependencyProperty.Register(nameof(NeuronBackground), typeof(Brush), typeof(UcNeuron));
        public static readonly DependencyProperty NeuronProperty = DependencyProperty.Register(nameof(Neuron), typeof(Neuron), typeof(UcNeuron));

        #endregion

        #region Properties + Indexers

        public Neuron Neuron
        {
            get => (Neuron)GetValue(NeuronProperty);
            set => SetValue(NeuronProperty, value);
        }

        public Brush NeuronBackground
        {
            get => (Brush)GetValue(NeuronBackgroundProperty);
            set => SetValue(NeuronBackgroundProperty, value);
        }

        #endregion

    }
}
