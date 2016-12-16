using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspTest.Genetic
{
    public class Population
    {
        public IList<Path> Paths { get; set; } = new List<Path>();
        public Path Fittest { get { return Paths.OrderByDescending(path => path.Fitness).First(); } }
        public Population(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Paths.Add(null);
            }
        }

        public Path this[int index]
        {
            get { return Paths[index]; }
            set { Paths[index] = value; }
        }

        public void Add(Path path)
        {
            this.Paths.Add(path);
        }

        public void Add(List<Path> paths)
        {
            paths.ForEach(Add);
        }

        public Population(IList<City> cities, int size = 10)
        {
            ISet<Path> paths = new HashSet<Path>();
            int generated = 0;
            Random rand = new Random();
            IEnumerable<int> indices = Enumerable.Range(0, cities.Count);
            while(generated < size)
            {
                var temp = indices.OrderBy(e => rand.Next());
                Path path = new Path(cities, temp.ToList());
                if(!paths.Contains(path))
                {
                    generated++;
                    this.Paths.Add(path);
                    paths.Add(path);
                }
            }
        }
    }
}
