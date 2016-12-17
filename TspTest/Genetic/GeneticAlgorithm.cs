using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspTest.Genetic
{
    public class GeneticAlgorithm
    {
        public Population Population { get; private set; }

        public GeneticAlgorithm(Population pop)
        {
            this.Population = pop;
        }

        private void _evolve()
        {
            var size = Population.Paths.Count;
            Population next = new Population();
            Random rand = new Random();
            var f = Population.Fittest.Fitness;
            var selected = Population.Paths.Where(e => e.Fitness > 5 * f / 10).ToList();
            next.Add(selected);
            int rest = size - selected.Count;
            for (int i = 0; i < rest; i++)
            {
                int k = rand.Next(0, selected.Count), j = rand.Next(0, selected.Count);
                while(k == j)
                {
                    k = rand.Next(0, selected.Count);
                    j = rand.Next(0, selected.Count);
                }
                var cross = _crossOver(selected[k], selected[j]);
                next.Add(cross);
            }
            //Console.WriteLine(next.ToString());
            for (int i = 0; i < size; i++)
            {
                _mutate(next[i]);
            }
            Population = next;
        }

        private void _evolveFor(int generations = 1000)
        {
            for (int i = 0; i < generations; i++)
            {
                _evolve();
            }
        }

        public Path Solve(int generations = 1000)
        {
            _evolveFor(generations);
            return Population.Fittest;
        }

        private Path _crossOver(Path first, Path second)
        {
            var count = first.Cities.Count;
            Random rand = new Random();
            //Path result = new Path(count);
            //int start = rand.Next(0, count - 1),
            //    end   = rand.Next(start + 1, count - start - 1);
            //for (int i = start; i <= end; i++)
            //{
            //    result[i] = first[i];
            //}
            //for (int i = 0; i < count; i++)
            //{
            //    result.AddIfNotContains(second[i], i);
            //}
            //for (int i = 0; i < count; i++)
            //{
            //    result.AddIfNotContains(first[i], i);
            //}
            //return result;
            Path child = new Path(count);
            int startPos = (int)(rand.NextDouble() * count);
            int endPos = (int)(rand.NextDouble() * count);
            for (int i = 0; i < count; i++)
            {
                if (startPos < endPos && i > startPos && i < endPos)
                {
                    child.AddIfNotContains(first[i], i);
                } 
                else if (startPos > endPos)
                {
                    if (!(i < startPos && i > endPos))
                    {
                        child.AddIfNotContains(first[i], i);
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                child.AddIfNotContains(second[i], i);
            }
            return child;
        }

        private void _mutate(Path path)
        {
            var count = path.Cities.Count;
            var rate = 0.08;
            Random rand = new Random();
            for (int i = 0; i < count; ++i)
            {
                if (rate > rand.NextDouble())
                {
                    int j = rand.Next(0, count);
                    if (i != j)
                    {
                        var temp = path.Cities[i];
                        path.Cities[i] = path.Cities[j];
                        path.Cities[j] = temp;
                    }
                }
            }
        }
    }
}
