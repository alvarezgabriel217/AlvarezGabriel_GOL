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
        public int value;
        public Randomize()
        {
            InitializeComponent();
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Minimum = int.MinValue;
            value = Properties.Settings.Default.Seed;
            numericUpDown1.Value = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            value = random.Next(int.MinValue, int.MaxValue);
            numericUpDown1.Value = value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            value = (int)numericUpDown1.Value;
        }
    }
}
