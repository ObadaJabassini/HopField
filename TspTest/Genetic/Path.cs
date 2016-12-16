using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspTest.Genetic
{
    public class Path
    {
        public IList<City?> Cities { get; set; } = new List<City?>();

        public Path(int length)
        {
            for (int i = 0; i < length; i++)
            {
                this.Cities.Add(null);
            }
        }

        public City this[int index]
        {
            get { return Cities[index].Value; }
            set { Cities[index] = value; }
        }
        public Path(IList<City> cities, IList<int> indices)
        {
            for (int i = 0; i < cities.Count; i++)
            {
                this.Cities[i] = cities[indices[i]];
            }
        }
        public double Fitness
        {
            get
            {
                if(_distance == 0)
                {
                    _distance = this.Cities.Zip(this.Cities.Skip(1), (f, s) => new Tuple<City?, City?>(f, s)).Select(e => e.Item1.Value.DistanceTo(e.Item2)).Sum();
                }
                return 1 / _distance;
            }
        }
        private double _distance = 0;

        public void AddIfNotContains(City city, int index)
        {
            if(!this.Cities.Contains(city))
            {
                this.Cities[index] = city;
            }
        }

        public override string ToString()
        {
            return this.Cities.Select(city => city.Value.Name).Aggregate((f, s) => f + " " + s);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return ((Path)obj).Cities.Equals(this.Cities);
        }

        public override int GetHashCode()
        {
            return this.Cities.Select(x => x.Value.Name).Aggregate((f, s) => f + s).GetHashCode();
        }
    }
}
