using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Windows.Diagrams.Core;
using Point = System.Drawing.Point;

namespace ANN.Resources
{
    class CityShape:RadDiagramShape
    {
        internal CityShape(Point postion):base()
        {
            this.Height = this.Width = 50;
            this.Text = "";
            this.BackgroundShape = new RadImageShape();
            this.BackgroundShape.Image = TspTest.Properties.Resources.cityscape;
            this.Position = postion;
            //this.Content = "llllllllllllll";
            this.CanFocus = true;
        }

        public override string ToString()
        {
            return Text+ "\t" + Location.ToString();
        }

        public override bool Focus()
        {
            return Focus(true);
        }
    }
}
