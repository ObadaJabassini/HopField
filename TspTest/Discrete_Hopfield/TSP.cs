using System;
using System.Collections.Generic;
using System.Drawing;
using Accord.Math;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace TspTest.Discrete_Hopfield
{
    struct City
    {
        public string Name { set; get; }
        public PointF Position { set; get; }
    }

    class TSP
    {
        private List<City> _cities { set; get; }
        public int CitiesNumber { get; }
        //public Matrix<double> D { get; }
        public double[,] D { get; }

        public TSP(List<City> cities)
        {
            _cities = cities;
            CitiesNumber = cities.Count;
            //D = Matrix<double>.Build.Dense(cities.Count, cities.Count);
            D = Matrix.Zeros(cities.Count, cities.Count);
            cities.ForEach(
                e1 =>
                    cities.ForEach(
                        e2 =>
                            D[cities.IndexOf(e1), cities.IndexOf(e2)] =
                                (int)
                                    Math.Sqrt(Math.Pow(e2.Position.X - e1.Position.X, 2) +
                                              Math.Pow(e2.Position.Y - e1.Position.Y, 2))));

            //Normalize
            //D.Divide(D.Enumerate().Maximum());
            //D.Apply(e => e / (D.Max()));
        }

        public void Solve()
        {
            HopfieldTank nn = new HopfieldTank(this);
            int e = 0;
            Tuple<double[,], List<double>> t = new Tuple<double[,], List<double>>(new double[,] {}, new List<double>());
            while (e++<1000)
            {
                t = nn.Analyze();
                //Print activations
                for (int i = 0; i < CitiesNumber; i++)
                {
                    for (int j = 0; j < CitiesNumber; j++)
                    {
                        Console.Write("{0,2} ", Math.Round(t.Item1[i, j]));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            t.Item2.ForEach(Console.WriteLine);
        }

        //private void _generateSolution(Tuple<Matrix<double>, List<double>> sol)
        private void _generateSolution(Tuple<double[,], List<double>> sol)
        {
            Console.WriteLine("Energy Values:");
            for (int i = 0; i < sol.Item2.Count; i++)
            {
                Console.WriteLine("Epoch: {0} => Energy = {1}", i+1, sol.Item2[i]);
            }
            Console.WriteLine("******************\nTour:");
            for (int i = 0; i < CitiesNumber; i++)
            {
                for (int j = 0; j < CitiesNumber; j++)
                {
                    if(sol.Item1[i+CitiesNumber*j, 0] == 1)
                        Console.WriteLine("Step: {0} => City: {1}", j, _cities[i].Name);
                }
            }
        }


    }
}
