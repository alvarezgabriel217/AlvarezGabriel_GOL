using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlvarezGabriel_GOL
{
    public partial class FormOptions : Form
    {
        // New Grid Width entered by the user
        static int gridWidth;
        // New Grid Height entered by the user
        static int gridHeight;
        // New timer interval entered by the user
        static int time;
        public FormOptions()
        {
            InitializeComponent();

            // Sets the values of the member fields to the values of the property settings
            Time = Properties.Settings.Default.Interval;
            GridWidth = Properties.Settings.Default.Width;
            GridHeight = Properties.Settings.Default.Height;

            // Sets the values of the numericupdowns to the values of the member fields
            numericUpDown1.Value = Time;
            numericUpDown2.Value = GridWidth;
            numericUpDown3.Value = GridHeight;

        }

        // Properties for the three member fields
        public int GridWidth
        {
            get {return gridWidth; }
            set {gridWidth = value; }
        } 
        public int GridHeight
        {
            get { return gridHeight; }
            set { gridHeight = value; }
        }

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        // Sets the newly entered values as the values of the member fields 
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Time = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            GridWidth = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            GridHeight = (int)numericUpDown3.Value;
        }
    }
}
