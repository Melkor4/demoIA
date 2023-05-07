using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using NeuralNetworkDemo.Library;
using NeuralNetworkDemo.Library.Objects;
using NeuralNetworkDemo.NeuralNetworkViewer.UI;

namespace NeuralNetworkDemo.NeuralNetworkViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();

            neuralNetworkBackgroundWorker = new BackgroundWorker();
            neuralNetworkBackgroundWorker.DoWork += NeuralNetworkBackgroundWorker_DoWork;
        }

        #endregion

        #region Fields

        private BackgroundWorker neuralNetworkBackgroundWorker;

        #endregion

        #region Private Methods

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            this.NeuralNetwork = AI.NeuralNetworks.GetNewSalaryNeuralNetwork();
            DisplayNetwork();
        }

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (this.NeuralNetwork == null)
                return;

            neuralNetworkBackgroundWorker.RunWorkerAsync();
        }

        private void BtnRandomize_Click(object sender, RoutedEventArgs e)
        {
            if (this.NeuralNetwork == null)
                return;

            if (!IudMin.Value.HasValue || !IudMax.Value.HasValue)
                return;

            AI.NeuralNetworks.RandomizeWeights(this.NeuralNetwork, IudMin.Value.Value, IudMax.Value.Value);
        }

        private void BtnRandomizeInput_Click(object sender, RoutedEventArgs e)
        {
            if (this.NeuralNetwork == null)
                return;

            Random random = new Random();

            int age = random.Next(14, 80);

            this.NeuralNetwork.NeuronLayers[0].Single(s => s.Label == "Âge").CurrentValue = age;
            this.NeuralNetwork.NeuronLayers[0].Single(s => s.Label == "Ancienneté").CurrentValue = random.Next(0, age - 14);
            this.NeuralNetwork.NeuronLayers[0].Single(s => s.Label == "Niveau Physique").CurrentValue = random.NextSingle();
            this.NeuralNetwork.NeuronLayers[0].Single(s => s.Label == "Niveau Intellectuel").CurrentValue = random.NextSingle();
        }

        private void DisplayNetwork()
        {
            // Clear the grid.
            GrdNetwork.Children.Clear();
            GrdNetwork.RowDefinitions.Clear();
            GrdNetwork.ColumnDefinitions.Clear();

            // No network = leave it empty.
            if (this.NeuralNetwork == null)
                return;

            Dictionary<Neuron, UcNeuron> uiDict = new Dictionary<Neuron, UcNeuron>();

            foreach (var layer in this.NeuralNetwork.NeuronLayers)
            {
                GrdNetwork.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                Grid layerGrid = new Grid();

                foreach (var neuron in layer)
                {
                    layerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                    Color neuronBg;
                    if (layer == this.NeuralNetwork.NeuronLayers.First())
                        neuronBg = Color.FromRgb(192, 207, 58); // yellow-green
                    else if (layer == this.NeuralNetwork.NeuronLayers.Last())
                        neuronBg = Color.FromRgb(9, 137, 177); // blue
                    else
                        neuronBg = Color.FromRgb(84, 158, 57); // green

                    UcNeuron ucNeuron = new UcNeuron
                    {
                        Neuron = neuron,
                        NeuronBackground = new SolidColorBrush(neuronBg)
                    };

                    uiDict.Add(neuron, ucNeuron);
                    layerGrid.Children.Add(ucNeuron);
                    Grid.SetRow(ucNeuron, layerGrid.Children.Count - 1);
                }
                GrdNetwork.Children.Add(layerGrid);
                Grid.SetColumn(layerGrid, GrdNetwork.Children.Count - 1);
            }

            Canvas canvas = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            GrdNetwork.Children.Insert(0, canvas);

            double columnWidth = GrdNetwork.ActualWidth / GrdNetwork.ColumnDefinitions.Count;
            System.Drawing.Color goldColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Gold);

            foreach (var link in this.NeuralNetwork.NeuronLinks)
            {
                if (link.LeftNeuron is null || link.RightNeuron is null)
                    continue;

                UcNeuron leftUcNeuron = uiDict[link.LeftNeuron];
                UcNeuron rightUcNeuron = uiDict[link.RightNeuron];

                int leftRow = Grid.GetRow(leftUcNeuron);
                int leftColumn = Grid.GetColumn((Grid)leftUcNeuron.Parent);
                int leftRowCount = ((Grid)leftUcNeuron.Parent).RowDefinitions.Count;

                int rightRow = Grid.GetRow(rightUcNeuron);
                int rightColumn = Grid.GetColumn((Grid)rightUcNeuron.Parent);
                int rightRowCount = ((Grid)rightUcNeuron.Parent).RowDefinitions.Count;

                Line line = new()
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(goldColor.R, goldColor.G, goldColor.B)),
                    StrokeThickness = 3,
                    X1 = (columnWidth * leftColumn) + (columnWidth / 1.33),
                    Y1 = leftRow * (GrdNetwork.ActualHeight / leftRowCount) + ((GrdNetwork.ActualHeight / leftRowCount) / 2),
                    X2 = (columnWidth * rightColumn) + (columnWidth / 1.33),
                    Y2 = rightRow * (GrdNetwork.ActualHeight / rightRowCount) + ((GrdNetwork.ActualHeight / rightRowCount) / 2)
                };

                canvas.Children.Insert(0, line);

                // Make a new source.
                Binding myBinding = new("Weigth")
                {
                    Source = link,
                    StringFormat = "F2"
                };

                TextBlock weightTextBlock = new()
                {
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                weightTextBlock.SetBinding(TextBlock.TextProperty, myBinding);

                // y = ax + b
                double a = (line.Y2 - line.Y1) / (line.X2 - line.X1);
                // line.Y1 = a(line.X1) + b
                // line.Y1 - a(line.X1) = a(line.X1) + b - a(line.X1)
                // line.Y1 - a(line.X1) = b
                double b = line.Y1 - (a * line.X1);

                double x = (Math.Min(line.X2, line.X1) + (Math.Abs(line.X2 - line.X1) / 4) - 25);

                Border border = new()
                {
                    Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Width = 50,
                    Height = 20,
                    Child = weightTextBlock,
                    Margin = new Thickness(x, ((x * a) + b) - 10, 0, 0)
                };

                canvas.Children.Add(border);
            }
        }

        private void NeuralNetworkBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (this.NeuralNetwork is null)
                return;

            AI.NeuralNetworks.ExecuteNetwork(this.NeuralNetwork, 250);
        }

        #endregion

        #region Properties + Indexers

        public NeuralNetwork? NeuralNetwork { get; set; }

        #endregion

    }
}
