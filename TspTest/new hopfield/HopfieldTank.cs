using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace TspTest.Discrete_Hopfield
{
    class HopfieldTank
    {
        private TSP _problem;
        private double u0 = 0.001; // this is a complete guess
        private double u0variation = 0.001;
        private double[,] u, V;
        private double A, B, D;
        private double dTimeInterval = 0.0001;
        private double dNormalize;
        private double E;
        private List<double> _energyValues = new List<double>();
        private int epochs;

        public HopfieldTank(TSP p, double A = 2.0/*1.2*/, double B = 2.0/*1.2*/, double D = 0.9/*1.5*/, int epochs = 10000)
        {
            _problem = p;
            this.A = A;
            this.B = B;
            this.D = D;
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
                    double ASum = -1.0, BSum = -1.0, DSum = 0.0;
                    for (int k = 0; k < _problem.CitiesNumber; k++)
                    {
                        if (k != i % n)
                            ASum += A * V[i / n, k];
                        if (k != i / n)
                            BSum += B * V[k, i % n];
                        if(i % n + 1 < n && i % n - 1 >= 0)
                            DSum += D * (float)(_problem.D[i / n, k] / dNormalize) * (V[k, i % n + 1] + V[k, i % n - 1]);
                    }

                    double dudt = (-ASum - BSum - DSum);
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
            double EBSum = 0.0, EASum = 0.0;
            for (int i = 0; i < _problem.CitiesNumber; i++)
            {
                EASum += Math.Abs(V.GetRow(i).Sum() - 1);
                EBSum += Math.Abs(V.GetColumn(i).Sum() - 1);
            }

            E = EASum + EBSum;
            _energyValues.Add(E);
        }
    }
}
