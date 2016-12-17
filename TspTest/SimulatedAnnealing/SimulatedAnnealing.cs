using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspTest.Genetic;

namespace TspTest.SimulatedAnnealing
{
    public class SimulatedAnnealing
    {
        private Matrix<double> _distances;
        private double _temperature;
        public SimulatedAnnealing(Matrix<double> distances, double temperature = 10000)
        {
            this._distances = distances;
            this._temperature = temperature;
        }

        public SimulatedAnnealing(IList<City> cities, double temperature = 10000)
        {
            this._temperature = temperature;
            var count = cities.Count;
            _distances = Matrix<double>.Build.Dense(count, count, 0);
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    _distances[i, j] = cities[i].DistanceTo(cities[j]);
                }
            }
        }

        public Tuple<IList<int>, double> Solve(double minTemp = 0.01, double coolingFactor = 0.923, double accProb = 0.4)
        {
            var n = _distances.RowCount;
            IList<int> path = Enumerable.Range(0, n).Shuffle().ToList();
            ISet<IList<int>> visited = new HashSet<IList<int>>();
            visited.Add(path);
            Random r = new Random();
            Func<IList<int>, IList<int>> generateNext = (p) =>
            {
                IList<int> next = new List<int>(p);
                while(visited.Contains(next))
                {
                    int i = r.Next(0, n), j = r.Next(0, n);
                    if(i != j)
                    {
                        var temp = next[i];
                        next[i] = next[j];
                        next[j] = temp;
                    }
                }
                return next;
            };
            Func<IList<int>, double> distance = (route) =>
            {
                double d = 0;
                for (int i = 0; i < route.Count - 1; i++)
                {
                    d += _distances[route[i], route[i + 1]];
                }
                return d;
            };
            Func<double, double> acceptanceProbability = (d) =>
            {
                if (d > 0)
                {
                    return accProb + 1;
                }
                return Math.Exp(d / _temperature);
            };
            while (_temperature > minTemp)
            {
                IList<int> nextSolution = generateNext(path);
                var delta = distance(path) - distance(nextSolution);
                if(acceptanceProbability(delta) > accProb)
                {
                    path = nextSolution;
                    visited.Add(path);
                }
                _temperature *= coolingFactor;
            }
            return new Tuple<IList<int>, double> (path.Select(x => x + 1).ToList(), distance(path));
        }
    }
}
