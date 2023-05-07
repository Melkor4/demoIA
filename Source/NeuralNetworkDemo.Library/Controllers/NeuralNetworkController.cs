using NeuralNetworkDemo.Library.Objects;
using NeuralNetworkDemo.Library.Objects.NeuralNetworkBuilders;

namespace NeuralNetworkDemo.Library.Controllers
{
    public class NeuralNetworkController
    {

        #region Internal Constructors

        internal NeuralNetworkController() { }

        #endregion

        #region Public Methods

        public void ExecuteNetwork(NeuralNetwork network, int millisecondsSleepInterval = 0)
        {
            // Flatten all the layers except the first one (which is the input layer).
            foreach (Neuron[] layer in network.NeuronLayers.Skip(1))
            {
                foreach (Neuron neuron in layer)
                    neuron.CurrentValue = 0;
            }

            // Execute the network.
            foreach (Neuron[] layer in network.NeuronLayers)
            {
                foreach (Neuron neuron in layer)
                {
                    foreach (NeuronLink link in network.NeuronLinks.Where(w => w.LeftNeuron == neuron))
                    {
                        if (link.RightNeuron != null && (link.ActivationFunction == null || (link.ActivationFunction != null && link.ActivationFunction.Invoke(neuron))))
                        {
                            link.RightNeuron.CurrentValue += neuron.CurrentValue * link.Weigth;
                            Thread.Sleep(millisecondsSleepInterval);
                        }
                    }
                }
            }
        }

        public NeuralNetwork GetNewSalaryNeuralNetwork() => new SalaryNeuralNetworkBuilder().Build();

        public void RandomizeWeights(NeuralNetwork network, int min, int max)
        {
            if (network == null)
                throw new ArgumentNullException(nameof(network));

            Random random = new Random();

            foreach (NeuronLink neuronLink in network.NeuronLinks)
                neuronLink.Weigth = random.Next(min, max) + random.NextDouble();
        }

        #endregion

    }
}
