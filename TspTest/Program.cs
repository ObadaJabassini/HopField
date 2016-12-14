using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspTest.Tsp;

namespace TspTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"D:/distances.txt");
            int n = Convert.ToInt32(lines[0]), index = 1;
            var distances = Matrix<double>.Build.Dense(n, n, 0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if(i != j)
                    { 
                        distances[i, j] = Convert.ToDouble(lines[index++]);
                    }
                }
            }
            Console.WriteLine();
            IList<int> result = new Hopfield(n, distances).Solve();
            Console.WriteLine(result.Skip(1).Select(x => x.ToString()).Aggregate((x, y) => x + " " + y));
            Console.ReadLine();
        }
    }
}
