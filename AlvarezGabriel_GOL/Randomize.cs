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
    public partial class Randomize : Form
    {
        int seed;

        public Randomize()
        {
            InitializeComponent();
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Minimum = int.MinValue;
            Seed = Properties.Settings.Default.Seed;
            numericUpDown1.Value = Seed;
        }
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            Seed = random.Next(int.MinValue, int.MaxValue);
            numericUpDown1.Value = seed;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            seed = (int)numericUpDown1.Value;
        }
    }
}
