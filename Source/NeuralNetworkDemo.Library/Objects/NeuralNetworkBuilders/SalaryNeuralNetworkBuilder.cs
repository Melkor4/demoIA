namespace NeuralNetworkDemo.Library.Objects.NeuralNetworkBuilders
{
    internal class SalaryNeuralNetworkBuilder : INeuralNetworkBuilder
    {

        #region Public Methods

        public NeuralNetwork Build()
        {
            NeuralNetwork neuralNetwork = new NeuralNetwork();

            Neuron[] inputLayer = new Neuron[]
            {
                new Neuron() { Label = "Âge", IsHumanInput=true },
                new Neuron() { Label = "Ancienneté", IsHumanInput=true },
                new Neuron() { Label = "Niveau Physique", IsHumanInput=true },
                new Neuron() { Label = "Niveau Intellectuel", IsHumanInput=true },
            };

            Neuron[] layer1 = new Neuron[]
            {
                new Neuron(),
                new Neuron(),
                new Neuron(),
                new Neuron(),
                new Neuron(),
                new Neuron(),
            };

            Neuron[] layer2 = new Neuron[]
            {
                new Neuron(),
                new Neuron(),
                new Neuron(),
            };

            Neuron[] outputLayer = new Neuron[]
            {
                new Neuron(),
            };

            neuralNetwork.NeuronLayers.Add(inputLayer);
            neuralNetwork.NeuronLayers.Add(layer1);
            neuralNetwork.NeuronLayers.Add(layer2);
            neuralNetwork.NeuronLayers.Add(outputLayer);

            foreach (Neuron neuron1 in inputLayer)
            {
                foreach (Neuron neuron2 in layer1)
                    neuralNetwork.NeuronLinks.Add(new NeuronLink() { LeftNeuron = neuron1, RightNeuron = neuron2 });
            }

            foreach (Neuron neuron1 in layer1)
            {
                foreach (Neuron neuron2 in layer2)
                    neuralNetwork.NeuronLinks.Add(new NeuronLink() { LeftNeuron = neuron1, RightNeuron = neuron2 });
            }

            foreach (Neuron neuron1 in layer2)
            {
                foreach (Neuron neuron2 in outputLayer)
                    neuralNetwork.NeuronLinks.Add(new NeuronLink() { LeftNeuron = neuron1, RightNeuron = neuron2 });
            }

            return neuralNetwork;
        }

        #endregion

    }
}
