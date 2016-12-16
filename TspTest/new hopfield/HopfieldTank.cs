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
        private double A, B, C, D;
        private double dTimeInterval = 0.0001;
        private double dNormalize;
        private double E;
        private List<double> _energyValues = new List<double>();

        public HopfieldTank(TSP p)
        {
            _problem = p;
            A = 1.2;
            B = 1.2;
            D = 1.5;
            
            Initialize(new Random().Next(99999));
        }

        internal void Initialize(int randomize)
        {
            dNormalize = _problem.D.Max();

            Random r = new Random(DateTime.Now.Millisecond + randomize);

            u =
                Matrix.Zeros(_problem.CitiesNumber, _problem.CitiesNumber)
                    .Apply(e => u0 + (r.Next(100)/100.0)*(u0variation*2.0) - u0variation);

            V = Matrix.Random(_problem.CitiesNumber, _problem.CitiesNumber);
        }

        public Tuple<double[,], List<double>> Analyze()
        {
            for (int i = 0; i < _problem.CitiesNumber; i++)
            {
                for (int j = 0; j < _problem.CitiesNumber; j++)
                {
                    double ASum = -1.0, BSum = -1.0, DSum = 0.0;
                    for (int k = 0; k < _problem.CitiesNumber; k++)
                    {
                        if(k != j)
                            ASum += 2.0 * V[i, k];
                        if(k != i)
                            BSum += 2.0 * V[k, j];
                        DSum += 0.9* (float) (_problem.D[i, k]/dNormalize)*
                                (V[k, (_problem.CitiesNumber + j + 1)%_problem.CitiesNumber] +
                                 V[k, (_problem.CitiesNumber + j - 1)%_problem.CitiesNumber]);
                    }

                    double dudt = (-ASum - BSum - DSum);
                    u[i, j] += dudt * dTimeInterval;
                }
            }

            for (int i = 0; i < _problem.CitiesNumber; i++)
            {
                for (int j = 0; j < _problem.CitiesNumber; j++)
                {
                    V[i, j] = 0.5 * (1.0 + Math.Tanh(u[i, j] / u0));
                }
            }
            _calculateEnergy();
            return new Tuple<double[,], List<double>>(V, _energyValues);
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
