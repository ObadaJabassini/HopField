using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TspTest.Genetic;

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

            //cityLocations(1,:) = [0 3];
            //cityLocations(2,:) = [1 5];
            //cityLocations(3,:) = [4 5];
            //cityLocations(4,:) = [5 2];
            //cityLocations(5,:) = [4 0];
            //cityLocations(6,:) = [1 0];
            List<City> l = new List<City>();
            //l.Add(new City { Name = "city1", Position = new PointF(0, 3) });
            //l.Add(new City { Name = "city2", Position = new PointF(1, 5) });
            //l.Add(new City { Name = "city3", Position = new PointF(4, 5) });
            //l.Add(new City { Name = "city4", Position = new PointF(5, 2) });
            //l.Add(new City { Name = "city5", Position = new PointF(4, 0) });
            //l.Add(new City { Name = "city6", Position = new PointF(1, 0) });

            l.Add(new City { Name = "city1", Position = new PointF(3, 3) });
            l.Add(new City { Name = "city2", Position = new PointF(4, 5) });
            l.Add(new City { Name = "city3", Position = new PointF(5, 1) });
            l.Add(new City { Name = "city4", Position = new PointF(7, 3) });

            //City city1 = new City { Name = "city1", Position = new PointF(8, 6) },
            //     city2 = new City { Name = "city2", Position = new PointF(0, 0) },
            //     city3 = new City { Name = "city3", Position = new PointF(35, 0) },
            //     city4 = new City { Name = "city4", Position = new PointF(4, 3) };
            //l.Add(city1);
            //l.Add(city2);
            //l.Add(city3);
            //l.Add(city4);
            //TSP prob = new TSP(l);
            //prob.Solve();

            //int index = 1;
            //File.ReadAllLines(@"D:/tspTest.txt").ToList().ForEach(line => { var vals = Regex.Split(line, " +").Select(e => Convert.ToDouble(e)).ToArray();
            //l.Add(new City() { Name = "City" + index++, Position = new PointF((float)vals[0], (float)vals[1]) } );});
            //Genetic.Path path = new GeneticAlgorithm(new Population(l, 48)).Solve(10000);
            //Console.WriteLine($"Path = {path.Cities.Select(c => c.Value.ToString()).Aggregate((f, s) => f + "-" + s)}\nCost = {path.Cities.Zip(path.Cities.Skip(1), (f, s) => new Tuple<City?, City?>(f, s)).Select(e => e.Item1.Value.DistanceTo(e.Item2.Value)).Sum()}");
            //Console.ReadKey();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RadForm2());
            TSP prob = new TSP(l);
            //prob.Solve();
            //int index = 1;
            //File.ReadAllLines(@"D:/tspTest.txt").ToList().ForEach(line => { var vals = Regex.Split(line, " +").Select(e => Convert.ToDouble(e)).ToArray(); l.Add(new City() { Name = "City" + index++, Position = new PointF((float)vals[0], (float)vals[1]) } );});
            //Genetic.Path path = new GeneticAlgorithm(new Population(l, 48)).Solve(10000);
            //Console.WriteLine($"Path = {path.Cities.Select(c => c.Value.ToString()).Aggregate((f, s) => f + "-" + s)}\nCost = {path.Cities.Zip(path.Cities.Skip(1), (f, s) => new Tuple<City?, City?>(f, s)).Select(e => e.Item1.Value.DistanceTo(e.Item2.Value)).Sum()}");
            Console.ReadKey();
        }
    }
}
