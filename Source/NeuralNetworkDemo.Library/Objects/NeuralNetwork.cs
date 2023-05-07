namespace NeuralNetworkDemo.Library.Objects
{
    public class NeuralNetwork
    {

        #region Properties + Indexers

        public List<Neuron[]> NeuronLayers { get; } = new List<Neuron[]>();

        public List<NeuronLink> NeuronLinks { get; } = new List<NeuronLink>();

        #endregion

    }
}
