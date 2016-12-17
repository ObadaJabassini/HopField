using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;

namespace TspTest
{
    class HopfieldTank
    {
        private TSP _problem;
        private double u0 = 0.001; // this is a complete guess
        private double u0variation = 0.001;
        private double[,] u, V;
        private double A, B, D, C; // this is a complete guess too :3
        private double dTimeInterval = 0.0001;
        private double dNormalize;
        private double E;
        private List<double> _energyValues = new List<double>();
        private int epochs;

        public HopfieldTank(TSP p, double A = 2.0, double B = 2.0, double C = 4, double D = 0.9, int epochs = 10000)
        {
            _problem = p;
            this.A = A;
            this.B = B;
            this.D = D;
            this.C = C;
            this.epochs = epochs;
            
            Initialize(new Random().Next(99999));
        }

        internal void Initialize(int randomize)
        {
            dNormalize = _problem.D.Max();

            Random r = new Random(DateTime.Now.Millisecond + randomize);

            u = Matrix.Zeros(_problem.CitiesNumber, _problem.CitiesNumber)
                    .Apply(e => u0 + (r.Next(100)/100.0)*(u0variation*2.0) - u0variation);

            V = Matrix.Random(_problem.CitiesNumber, _problem.CitiesNumber);
        }

        public Tuple<double[,], List<double>> Analyze()
        {
            IEnumerable<int> indices = _getRandomIndices();
            int n = _problem.CitiesNumber;
            bool systemChanged = true;
            int e = 0;
            while (systemChanged && e++ < epochs)
            {
                systemChanged = false;
                foreach (var i in indices)
                {
                    double ASum = 0.0, BSum = 0.0, DSum = 0.0, CSum = 0.0;
                    for (int k = 0; k < _problem.CitiesNumber; k++)
                    {
                        if (k != i % n)
                            ASum += A * V[i / n, k];
                        if (k != i / n)
                            BSum += B * V[k, i % n];
                        if(i % n + 1 < n && i % n - 1 >= 0)
                            DSum += D * (float)(_problem.D[i / n, k] / dNormalize) * (V[k, i % n + 1] + V[k, i % n - 1]);
                    }

                    double dudt = (-ASum - BSum - DSum - C*(V.Sum() - n));
                    u[i / n, i % n] += dudt * dTimeInterval;
                    double oldV = V[i / n, i % n];
                    V[i / n, i % n] = 0.5 * (1.0 + Math.Tanh(u[i / n, i % n] / u0));
                    if (Math.Abs(oldV - V[i/n, i%n]) > 0.0001)
                        systemChanged = true;
                }

                _calculateEnergy();
                
            }
            return new Tuple<double[,], List<double>>(V, _energyValues);
        }

        private IEnumerable<int> _getRandomIndices()
        {
            IEnumerable<int> set = Enumerable.Range(0, (_problem.CitiesNumber) * (_problem.CitiesNumber));
            Random rand = new Random();
            IEnumerable<int> indices = set.OrderBy(e => rand.Next());
            return indices;
        }

        private void _calculateEnergy()
        {
            Func<double[,], int, int, double> availableActivation = (mat, i, j) => j >= 0 && j < mat.Columns() ? mat[i, j] : 0;
            int n = _problem.CitiesNumber;
            double term1 = 0, term2 = 0, term3 = 0, term4 = 0;
            for (int i = 0; i < _problem.CitiesNumber; i++)
            {
                for (int j = 0; j < _problem.CitiesNumber; j++)
                {
                    term3 += V[i, j] - _problem.CitiesNumber;
                    for (int k = 0; k < _problem.CitiesNumber; k++)
                    {
                        if (j != k)
                        {
                            term1 += V[i, j] * V[i, k];
                            term2 += V[j, i] * V[k, i];
                        }
                        if (i != j)
                            term4 += _problem.D[i, j] * V[i, k] *
                                     (availableActivation(V, j, k+1) +
                                      availableActivation(V, j, k-1));

                    }
                }
            }

            _energyValues.Add(1 / 2.0 * (A * term1 + B * term2 + C * term3 * term3 + D * term4));
        }
    }
}
