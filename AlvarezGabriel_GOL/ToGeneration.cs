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
        // St
        int generations;
        public ToGeneration()
        {
            InitializeComponent();
            Form1 form = new Form1();
            Generations = form.Generations;
            numericUpDown1.Value = Generations;
        }

        public int Generations 
        {
            get { return generations; }
            set { generations = value; } 
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Generations = (int)numericUpDown1.Value;
        }
    }
}
