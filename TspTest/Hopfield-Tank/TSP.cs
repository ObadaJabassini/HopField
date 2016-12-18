using System;
using System.Collections.Generic;
using System.Drawing;
using Accord.Math;

namespace TspTest
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
        public double[,] D { get; }

        public TSP(List<City> cities)
        {
            _cities = cities;
            CitiesNumber = cities.Count;
            D = Matrix.Zeros(cities.Count, cities.Count);
            cities.ForEach(
                e1 =>
                    cities.ForEach(
                        e2 =>
                            D[cities.IndexOf(e1), cities.IndexOf(e2)] =
                                (int)
                                    Math.Sqrt(Math.Pow(e2.Position.X - e1.Position.X, 2) +
                                              Math.Pow(e2.Position.Y - e1.Position.Y, 2))));
        }

        public Tuple<double[,], List<double>> Solve()
        {
            HopfieldTank nn = new HopfieldTank(this);
            return _generateSolution(nn.Analyze());
            
        }

        private Tuple<double[,], List<double>> _generateSolution(Tuple<double[,], List<double>> sol)
        {
            Console.WriteLine("Energy Values:");
            for (int i = 0; i < sol.Item2.Count; i++)
            {
                Console.WriteLine("Epoch: {0} => Energy = {1}", i + 1, sol.Item2[i]);
            }
            //gui
            double[,] trans = new double[CitiesNumber, CitiesNumber];
            Console.WriteLine("******************\nTour:");
            for (int i = 0; i < CitiesNumber; i++)
            {
                for (int j = 0; j < CitiesNumber; j++)
                {
                    Console.Write("{0,2} ", Math.Round(sol.Item1[i, j]));
                    //
                    trans[i, j] = Math.Round(sol.Item1[i, j]);

                }
                Console.WriteLine();
            }
            return new Tuple<double[,], List<double>>(trans,sol.Item2);
        }

    }
}
