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
        static int gridWidth;
        static int gridHeight;
        static int time;
        public FormOptions()
        {
            InitializeComponent();

            Time = Properties.Settings.Default.Interval;
            GridWidth = Properties.Settings.Default.Width;
            GridHeight = Properties.Settings.Default.Height;

            numericUpDown1.Value = Time;
            numericUpDown2.Value = GridWidth;
            numericUpDown3.Value = GridHeight;

        }
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
