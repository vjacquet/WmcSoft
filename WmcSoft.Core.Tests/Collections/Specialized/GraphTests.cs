using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class GraphTests
    {
        #region Testing samples

        static Graph TinyG() {
            var g = new Graph(13);
            g.Connect(0, 5);
            g.Connect(4, 3);
            g.Connect(0, 1);
            g.Connect(9, 12);
            g.Connect(6, 4);
            g.Connect(5, 4);
            g.Connect(0, 2);
            g.Connect(11, 12);
            g.Connect(9, 10);
            g.Connect(0, 6);
            g.Connect(7, 8);
            g.Connect(9, 11);
            g.Connect(5, 3);
            return g;
        }

        static Graph TinyCG() {
            var g = new Graph(6);
            g.Connect(0, 5);
            g.Connect(2, 4);
            g.Connect(2, 3);
            g.Connect(1, 2);
            g.Connect(0, 1);
            g.Connect(3, 4);
            g.Connect(3, 5);
            g.Connect(0, 2);
            return g;
        }

        class JobNames
        {
            public int Algorithms = 0;
            public int Databases = 1;
            public int IntroductionToCS = 2;
            public int AdvancedProgramming = 3;
            public int ComputationalBiology = 4;
            public int ScientificComputing = 5;
            public int TheoricalCS = 6;
            public int LinearAlgebra = 7;
            public int Calculus = 8;
            public int ArtificialInterlligence = 9;
            public int Robotics = 10;
            public int MachineLearning = 11;
            public int NeuralNetworks = 12;
        }


        static Digraph Jobs() {
            var g = new Digraph(13);
            var j = new JobNames();
            g.Connect(j.Algorithms, j.TheoricalCS, j.Databases, j.ScientificComputing);
            g.Connect(j.IntroductionToCS, j.AdvancedProgramming, j.Algorithms);
            g.Connect(j.AdvancedProgramming, j.ScientificComputing);
            g.Connect(j.ScientificComputing, j.ComputationalBiology);
            g.Connect(j.TheoricalCS, j.ComputationalBiology, j.ArtificialInterlligence);
            g.Connect(j.LinearAlgebra, j.TheoricalCS);
            g.Connect(j.Calculus, j.LinearAlgebra);
            g.Connect(j.ArtificialInterlligence, j.NeuralNetworks, j.Robotics, j.MachineLearning);
            g.Connect(j.MachineLearning, j.NeuralNetworks);
            return g;
        }

        #endregion

        void CheckDepthFirstSearch(Graph graph, int v, bool connected, params int[] expected) {
            var marked = graph.DepthFirstSearch(v);
            Assert.AreEqual(connected, marked.Count == graph.VerticeCount);

            var connections = Enumerable.Range(0, graph.VerticeCount).Where(i => marked[i]).ToList();
            CollectionAssert.AreEquivalent(connections, expected);
        }

        [TestMethod]
        public void CheckDepthFirstSearch() {
            var graph = TinyG();

            CheckDepthFirstSearch(graph, 0, false, 0, 1, 2, 3, 4, 5, 6);
            CheckDepthFirstSearch(graph, 9, false, 9, 10, 11, 12);
        }

        public void CheckPaths(DepthFirstPathsAlgorithm paths, int v, params int[] expected) {
            CollectionAssert.AreEqual(paths.PathTo(v).ToList(), expected);
        }

        [TestMethod]
        public void CheckDepthFirstPaths() {
            var graph = TinyCG();

            var paths = graph.DepthFirstPaths(0);
            CheckPaths(paths, 0, 0);
            CheckPaths(paths, 1, 0, 2, 1);
            CheckPaths(paths, 2, 0, 2);
            CheckPaths(paths, 3, 0, 2, 3);
            CheckPaths(paths, 4, 0, 2, 3, 4);
            CheckPaths(paths, 5, 0, 2, 3, 5);
        }

        public void CheckPaths(BreathFirstPathsAlgorithm paths, int v, params int[] expected) {
            CollectionAssert.AreEqual(paths.PathTo(v).ToList(), expected);
        }

        [TestMethod]
        public void CheckBreathFirstPaths() {
            var graph = TinyCG();

            var paths = graph.BreathFirstPaths(0);
            CheckPaths(paths, 0, 0);
            CheckPaths(paths, 1, 0, 1);
            CheckPaths(paths, 2, 0, 2);
            CheckPaths(paths, 3, 0, 2, 3);
            CheckPaths(paths, 4, 0, 2, 4);
            CheckPaths(paths, 5, 0, 5);
        }

        public void CheckComponents(ConnectedComponentsAlgorithm cc, int id, params int[] expected) {
            CollectionAssert.AreEqual(cc.Components(id).ToList(), expected);
        }

        [TestMethod]
        public void CheckConnectedComponents() {
            var graph = TinyG();

            var cc = graph.ConnectedComponents();
            CheckComponents(cc, 0, 0, 1, 2, 3, 4, 5, 6);
            CheckComponents(cc, 1, 7, 8);
            CheckComponents(cc, 2, 9, 10, 11, 12);
        }

        [TestMethod]
        public void CheckTopological() {
            var graph = Jobs();
            var topological = graph.Topological();
            var j = new JobNames();

            var expected = new int[] {
                j.Calculus,
                j.LinearAlgebra,
                j.IntroductionToCS,
                j.AdvancedProgramming,
                j.Algorithms,
                j.TheoricalCS,
                j.ArtificialInterlligence,
                j.Robotics,
                j.MachineLearning,
                j.NeuralNetworks,
                j.Databases,
                j.ScientificComputing,
                j.ComputationalBiology,
            };
            var actual = topological.Order.ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
