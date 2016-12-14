using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspTest.Discrete_Hopfield;
using TspTest.Tsp;

namespace TspTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            //string[] lines = File.ReadAllLines(@"D:/distances.txt");
            //int n = Convert.ToInt32(lines[0]), index = 1;
            //var distances = Matrix<double>.Build.Dense(n, n, 0);
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //    {
            //        if(i != j)
            //        { 
            //            distances[i, j] = Convert.ToDouble(lines[index++]);
            //        }
            //    }
            //}
            //Console.WriteLine();
            ////IList<int> result = new Hopfield(n, distances).Solve();
            //var result = new SimulatedAnnealing.SimulatedAnnealing(distances).Solve();
            //Console.WriteLine($"Path = {result.Item1.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y)}\nCost = {result.Item2}");
            //Console.ReadLine();


            List<City> l = new List<City>();
            l.Add(new City { Name = "city1", Position = new PointF(3, 3) });
            l.Add(new City { Name = "city2", Position = new PointF(4, 5) });
            l.Add(new City { Name = "city3", Position = new PointF(5, 1) });
            l.Add(new City { Name = "city4", Position = new PointF(7, 3) });

            TSP prob = new TSP(l);
            prob.Solve();

            Console.ReadKey();
        }
    }
}
