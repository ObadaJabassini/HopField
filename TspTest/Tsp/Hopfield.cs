using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace TspTest.Tsp
{
    public class Hopfield
    {
        private Matrix<double> _distances;
        private Func<int, int, int, int> to1D = (row, column, width) => row * width + column;
        private Func<Matrix<double>, int, int, double> checkIndex = (Matrix<double> vv, int u, int v) => u >= 0 && u < vv.RowCount && v >= 0 && v < vv.ColumnCount ? vv[u, v] : 0;
        private double A, B, C, D, o, alpha;
        private int numberOfCities;
        private Matrix<double> _weights;
        public Hopfield(int numberOfCities, Matrix<double> distances, double A = 500, double B = 500, double C = 200, double D = 500, double o = 0, double alpha = 3)
        {
            this._distances = distances;
            var t = numberOfCities * numberOfCities;
            this.numberOfCities = numberOfCities;
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
            this.o = o;
            _weights = Matrix<double>.Build.Dense(numberOfCities * numberOfCities, numberOfCities * numberOfCities);
            Func<int, int, int> delta = (v, vv) => v == vv ? 1 : 0;
            for (int i = 0; i < numberOfCities; i++)
            {
                for (int k = 0; k < numberOfCities; k++)
                {
                    for (int l = 0; l < numberOfCities; l++)
                    {
                        for (int j = 0; j < numberOfCities; j++)
                        {
                            _weights[to1D(i, k, numberOfCities), to1D(l, j, numberOfCities)] = -A * delta(i, l) * (1 - delta(k, j)) - B * delta(k, j) * (1 - delta(j, l)) - C - D * (_distances[j, l] * (delta(j, k - 1) + delta(j, k + 1)));
                        }
                    }
                }
            }
            this.alpha = alpha;
        }
        
        public IList<int> Solve()
        {
            Vector<double> currentInput = Vector<double>.Build.Random(numberOfCities * numberOfCities, new MathNet.Numerics.Distributions.Beta(5, 5));
            Matrix<double> X = Matrix<double>.Build.Random(numberOfCities, numberOfCities, new MathNet.Numerics.Distributions.Beta(2, 2));            
            int iterations = 1;
            IEnumerable<int> indices = Enumerable.Range(0, numberOfCities);
            Random rnd = new Random();
            double prevEnergy = 0, energy = 1;
            while (iterations++ <= 10000 && prevEnergy != energy)
            {
                prevEnergy = energy;
                IEnumerable<int> firstIndexArray = indices.OrderBy(x => rnd.Next()),
                                 secondIndexArray = indices.OrderBy(x => rnd.Next());
                foreach (var i in firstIndexArray)
                {
                    foreach (var j in secondIndexArray)
                    {
                        var index = to1D(i, j, numberOfCities);
                        var vec = Vector<double>.Build.Dense(numberOfCities * numberOfCities, 0);
                        int dd = 0;
                        for (int k = 0; k < X.RowCount; k++)
                        {
                            for (int v = 0; v < X.ColumnCount; v++)
                            {
                                vec[dd++] = X[k, v];
                            }
                        }
                        currentInput[index] = _weights.Row(to1D(i, j, X.ColumnCount)) * vec;
                        //currentInput[index] = -(currentInput[index] + A * X.Row(i).Sum() + B * X.Column(j).Sum() + C * (X.RowSums().Sum() - numberOfCities) + D * _distances.Row(i).EnumerateIndexed().Select(xx => xx.Item2 * (checkIndex(X, xx.Item1, j + 1) + checkIndex(X, xx.Item1, j - 1))).Sum());
                        //X[i, j] = (1 + Math.Tanh(this.alpha * currentInput[index])) / 2;
                        X[i, j] = currentInput[index] > 0 ? 1 : 0;
                    }
                }
                energy = _energy(X);
                Console.WriteLine("X :\n" + X);
                Console.WriteLine("Energy = " + energy);
            }
            //Matrix<double> X = Matrix<double>.Build.Random(numberOfCities, numberOfCities, new MathNet.Numerics.Distributions.Beta(2, 2));
            //double prevEnergy = 0, energy = _energy(X);
            //int iterations = 1;
            //IEnumerable<int> indices = Enumerable.Range(0, numberOfCities);
            //Random rnd = new Random();
            //int internalLoop = 5 * numberOfCities * numberOfCities;
            //while (prevEnergy != energy && iterations++ <= 10000)
            //{
            //    prevEnergy = energy;
            //    for (int vv = 1; vv <= internalLoop; ++vv)
            //    {
            //        IEnumerable<int> firstIndexArray = indices.OrderBy(x => rnd.Next()),
            //                         secondIndexArray = indices.OrderBy(x => rnd.Next());
            //        foreach (var i in firstIndexArray)
            //        {
            //            foreach (var j in secondIndexArray)
            //            {
            //                var index = to1D(i, j, numberOfCities);
            //                currentInput[index] = -A * X.Row(i).EnumerateIndexed().Where(xx => xx.Item1 != j).Select(xx => xx.Item2).Sum()
            //                           - B * X.Column(j).EnumerateIndexed().Where(xx => xx.Item1 != i).Select(xx => xx.Item2).Sum()
            //                           - C * (X.RowSums().Sum() - (numberOfCities + o))
            //                           - D * _distances.Row(i).EnumerateIndexed().Select(xx => xx.Item2 * (checkIndex(X, xx.Item1, j + 1) + checkIndex(X, xx.Item1, j - 1))).Sum();
            //                X[i, j] = (1 + Math.Tanh(this.alpha * currentInput[index])) / 2;
            //            }
            //        }
            //        Console.WriteLine("X : " + X);
            //    }
            //    energy = _energy(X);
            //    Console.WriteLine("Energy = " + energy);
            //}
            Console.WriteLine("Result X :" + X);
            X = X.Map(e => Math.Round(e));
            IList<int> path = Enumerable.Repeat(0, numberOfCities + 1).ToList();
            for (int i = 0; i < X.RowCount; i++)
            {
                for (int j = 0; j < X.ColumnCount; j++)
                {
                    if (X[i, j] == 1)
                        path[j + 1] = i + 1;
                }
            }
            return path;
        }

        private double _energy(Matrix<double> v)
        {
            double temp1 = 0, temp2 = 0, temp3 = 0;
            for (int x = 0; x < numberOfCities; x++)
            {
                for (int i = 0; i < numberOfCities; i++)
                {
                    for (int j = 0; j < numberOfCities; j++)
                    {
                        if (i != j)
                        {
                            temp1 += v[x, i] * v[x, j];
                            temp2 += v[i, x] * v[j, x];
                        }
                        if (x != i)
                        {
                            temp3 += _distances[x, i] * v[i, j] * (checkIndex(v, i, j - 1) + checkIndex(v, i, j + 1));
                        }
                    }
                }
            }
            return (A * temp1 + B * temp2 + D * temp3 + C * Math.Pow(v.RowSums().Sum() - (numberOfCities + o), 2)) / 2;
        }
    }
}
