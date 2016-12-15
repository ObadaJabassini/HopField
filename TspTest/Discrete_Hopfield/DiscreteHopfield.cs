using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;

namespace TspTest.Discrete_Hopfield
{
    class DiscreteHopfield
    {
        private Matrix<double> _activations;
        private Matrix<double> _weights;
        //private Matrix<double> _biases; 
        private TSP _problem;
        private List<double> _energyValues = new List<double>(); 
        public double A { get; }
        public double B { get; }
        public double D { get; }
        public double C { get; }
        public double N { get; }
        public double Epochs { get; set; }

        public DiscreteHopfield(TSP prop, double A = 500, double B = 500, 
            double D = 500, double C = 200, double N = 15, double Epochs = 100000)
        {
            this.A = A;
            this.B = B;
            this.D = D;
            this.C = C;
            this.N = N;
            this.Epochs = Epochs;
            _problem = prop;

            _activations = Matrix<double>.Build.Dense(prop.CitiesNumber* prop.CitiesNumber, 1, -1);
            _weights = Matrix<double>.Build.Dense(prop.CitiesNumber * prop.CitiesNumber,
                prop.CitiesNumber * prop.CitiesNumber);
            //_biases = Matrix<double>.Build.Dense(1, prop.CitiesNumber* prop.CitiesNumber, C*N);
            _initWeights();
        }

        private void _initWeights()
        {
            Func<int, int, int> d = (i, j) => Convert.ToInt32(i == j);
            int n = _problem.CitiesNumber;
            for (int i = 0; i < _weights.RowCount; i++)
            {
                for (int j = 0; j < _weights.ColumnCount; j++)
                {
                    if (i / n == j / n && i % n != j % n)
                        _weights[i, j] = -2;
                    else if (i / n != j / n && i % n == j % n)
                        _weights[i, j] = -2;
                    else if (i / n != j / n && i % n != j % n && Math.Abs(i % n - j % n) == 1)
                        _weights[i, j] = -_problem.D[i / n, j / n];
                    
                    //Or
                    //_weights[i, j] = -2 * d(i / n, j / n) * (1 - d(i % n, j % n))
                    //                 - 2 * d(i % n, j % n) * (1 - d(i / n, j / n))
                    //                 - _problem.D[i / n, j / n] * (1 - d(i % n, j % n)) * (1 - d(i / n, j / n)) * (d(j % n, i % n + 1) + d(j % n, i % n - 1));
                }
            }
        }

        public Tuple<Matrix<double>, List<double>> Iterate()
        {
            IEnumerable<int> indices = _getRandomIndices();
            //threshold = 20
            Func<double, double> f = (ui) => ui >= 20 ? 1 : -1;
            bool systemChanged = true;
            int e = 0;
            while (systemChanged && e++<Epochs)
            {
                systemChanged = false;
                foreach (var i in indices)
                {
                    double oldActivation = _activations[i, 0];
                    _activations[i, 0] = f((_weights.Row(i).ToRowMatrix()*_activations)[0, 0]);
                    if (oldActivation != _activations[i, 0])
                        systemChanged = true;
                }

                _calculateEnergy();

                //Print activations
                for (int i = 0; i < _problem.CitiesNumber; i++)
                {
                    for (int j = 0; j < _problem.CitiesNumber; j++)
                    {
                        Console.Write("{0,2} ",_activations[i+_problem.CitiesNumber*j, 0]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            return new Tuple<Matrix<double>, List<double>>(_activations, _energyValues);
        }

        private IEnumerable<int> _getRandomIndices()
        {
            IEnumerable<int> set = Enumerable.Range(0, (_problem.CitiesNumber - 1)*(_problem.CitiesNumber - 1));
            Random rand = new Random();
            IEnumerable<int> indices = set.OrderBy(e => rand.Next());
            return indices;
        }

        private void _calculateEnergy()
        {
            Func<Matrix<double>, int, double> availableActivation = (mat, i) => i >= 0 && i < mat.RowCount ? mat[i, 0] : -1;
            int n = _problem.CitiesNumber;
            double term1 = 0, term2 = 0, term3 = 0, term4 = 0;
            for (int i = 0; i < _problem.CitiesNumber; i++)
            {
                for (int j = 0; j < _problem.CitiesNumber; j++)
                {
                    term3 += _activations[i*n+j, 0] - _problem.CitiesNumber;
                    for (int k = 0; k < _problem.CitiesNumber; k++)
                    {
                        if (j != k)
                        {
                            term1 += _activations[j*n + i, 0]*_activations[k*n + i, 0];
                            term2 += _activations[i*n + j, 0]*_activations[i*n + k, 0];
                        }
                        if (i != j)
                            term4 += _problem.D[i, j]*_activations[i*n + k, 0]*
                                     (availableActivation(_activations, j*n + i + 1) +
                                      availableActivation(_activations, j*n + i - 1));

                    }
                }
            }
            
            _energyValues.Add(1/2.0*(A*term1+B*term2+C*term3*term3+D*term4));
        } 

    }
}
