using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Accord.Math;
using ANN.Resources;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Windows.Diagrams.Core;
using TspTest.Discrete_Hopfield;
using City = TspTest.Genetic.City;
using Point = System.Drawing.Point;

namespace TspTest
{
    public partial class RadForm2 : Telerik.WinControls.UI.RadForm
    {
        private ShapeCollection Cities
        {
            set { }
            get { return Map.Shapes; }

        }

        private int CountCities
        {
            set { }
            get { return Cities.Count; }
        }

        List<TspTest.Discrete_Hopfield.City> l = new List<TspTest.Discrete_Hopfield.City>();
        Point MousPosition = new Point();

        public RadForm2()
        {
            InitializeComponent();
        }
        private void CityOnDrag(object sender, EventArgs eventArgs)
        {
            CityShape city = sender as CityShape;
            //city.Location=MousPosition;
            if (Map.IsDraggingEnabled)
            {
                this.radListView1.Items[Cities.IndexOf(city)] = new ListViewDataItem(city.ToString());
            }
        }
        private void Map_Click(object sender, EventArgs e)
        {
            if (AddCity_tgl.IsChecked)
            {
                CityShape city = new CityShape(MousPosition);
                city.Click += this.CityOnDrag;
                Map.AddShape(city);
                l.Add(new TspTest.Discrete_Hopfield.City { Name = "City"+(CountCities-1), Position = MousPosition });
            }
        }

        private void Map_ItemsChanged(object sender, Telerik.WinControls.UI.Diagrams.DiagramItemsChangedEventArgs e)
        {
            radListView1.Items.Clear();
            for (int i = 0; i < CountCities; i++)
            {
                CityShape city = Cities[i] as CityShape;
                radListView1.Items.Add(i + " " + city.ToString());
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            double zoom = this.Map.Zoom;
            MousPosition = new Point((int)((MousePosition.X  - this.Location.X - 32) + zoom),
               (int) (( MousePosition.Y - this.Location.Y - 110)+zoom));
            MouseCoords_lbl.Text = MousPosition.ToString();
        }

        private void AddCity_tgl_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (AddCity_tgl.IsChecked)
            {
                Map.ActiveTool = MouseTool.PointerTool;
                FalseOthers(AddCity_tgl);
            }
        }
        void FalseOthers(RadToggleButton checkedButton)
        {
            foreach (var control in this.tools.Controls)
            {
                if (checkedButton != control)
                {
                    RadToggleButton toggleButton = control as RadToggleButton;
                    toggleButton.IsChecked = false;
                }
            }
            Map.IsDraggingEnabled = false;
        }

        private void Pan_tgl_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (Pan_tgl.IsChecked)
            {
                Map.ActiveTool = MouseTool.PanTool;
                FalseOthers(Pan_tgl);
            }
        }

        private void MoveCity_tgl_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (MoveCity_tgl.IsChecked)
            {
                Map.ActiveTool = MouseTool.PointerTool;
                FalseOthers(MoveCity_tgl);
                Map.IsDraggingEnabled = true;
            }
            else
            {
                Map.IsDraggingEnabled = false;
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            TSP prob = new TSP(l);
            double[,] solution =prob.Solve();

            for (int j = 0; j < CountCities; j++)
            {
                for (int i = 0; i < CountCities; i++)
                {   
                    if (solution[i, j] == 1)
                    {
                        Map.AddConnection(j == 0 ? Cities[i] : Map.Connections[Map.Connections.Count - 1].Target,
                            Cities[i]);

                        RadDiagramConnection connection1 = (RadDiagramConnection)Map.Connections[Map.Connections.Count - 1];
                        connection1.BackColor = Color.LightSalmon;
                        connection1.AllowDelete = false;
                        connection1.IsDraggingEnabled = false;
                        connection1.IsEditable = false;
                        connection1.TargetCapSize = new SizeF(20, 25);
                        connection1.IsHitTestVisible = true;
                        connection1.Content = j + 1;
                        connection1.ForeColor = Color.LightSalmon;
                        connection1.Font = MouseCoords_lbl.Font;
                        Map.Refresh();
                        //connection1.Position = connection1.Target.Position;
                        System.Windows.Forms.Application.DoEvents();
                        break;
                    }
                }
            }
            //int k = 0;
            //foreach (RadDiagramConnection road in Map.Connections)
            //{
            //    //Console.WriteLine(road.Source.ToString()+road.Target.ToString() );
            //    Console.WriteLine(road.Position);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void radPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radToggleButton2_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {

        }
    }
}
