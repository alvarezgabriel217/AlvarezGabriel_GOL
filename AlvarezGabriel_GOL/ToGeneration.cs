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
    public partial class ToGeneration : Form
    {
        public int generations;
        public ToGeneration()
        {
            InitializeComponent();
            Form1 form = new Form1();
            generations = form.Generations;
            numericUpDown1.Value = generations;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            generations = (int)numericUpDown1.Value;
        }
    }
}
